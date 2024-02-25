using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ProjectK.Api.Attributes;
using ProjectK.Api.Filters;
using ProjectK.Core.Adapters.Driven.Database;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Database.Contexts;

namespace ProjectK.Api.Tests.Filters;

public class TransactionFilterTests
{
    private readonly INotificationManager _notificationManager;
    private readonly TransactionFilter _transactionFilter;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionFilterTests()
    {
        var logger = Substitute.For<ILogger<TransactionFilter>>();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("ProjectK")
            .Options;
        var appDbContext = new AppDbContext(options, Substitute.For<IUserContext>());
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _notificationManager = Substitute.For<INotificationManager>();

        _transactionFilter = new TransactionFilter(logger, appDbContext, _unitOfWork, _notificationManager);
    }

    [Fact]
    public async Task It_should_commit_when_transaction_attribute_exists_and_no_notifications_are_present()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );

        var endpointMetadata = new List<object> { new TransactionAttribute() };

        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>()!,
            Substitute.For<Controller>()
        )
        {
            Result = null
        };
        var newEndpointMetadata = new List<object>(actionExecutingContext.ActionDescriptor.EndpointMetadata);
        newEndpointMetadata.AddRange(endpointMetadata);

        actionExecutingContext.ActionDescriptor.EndpointMetadata = newEndpointMetadata;

        var next = Substitute.For<ActionExecutionDelegate>();

        next
            .Invoke()
            .Returns(
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Substitute.For<Controller>()));

        // Act
        await _transactionFilter.OnActionExecutionAsync(actionExecutingContext, next);

        // Assert
        await _unitOfWork
            .Received(1)
            .BeginTransactionAsync(actionContext.HttpContext.RequestAborted);
        await _unitOfWork
            .Received(1)
            .CommitAsync(actionContext.HttpContext.RequestAborted);
    }

    [Fact]
    public async Task It_should_rollback_if_notifications_exist()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );

        var endpointMetadata = new List<object> { new TransactionAttribute() };

        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>()!,
            Substitute.For<Controller>()
        )
        {
            Result = null
        };
        var newEndpointMetadata = new List<object>(actionExecutingContext.ActionDescriptor.EndpointMetadata);
        newEndpointMetadata.AddRange(endpointMetadata);

        actionExecutingContext.ActionDescriptor.EndpointMetadata = newEndpointMetadata;

        _notificationManager.HasNotifications.Returns(true);

        var next = Substitute.For<ActionExecutionDelegate>();

        next
            .Invoke()
            .Returns(
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Substitute.For<Controller>()));

        // Act
        await _transactionFilter.OnActionExecutionAsync(actionExecutingContext, next);

        // Assert
        await _unitOfWork
            .Received(1)
            .BeginTransactionAsync(actionContext.HttpContext.RequestAborted);
        await _unitOfWork
            .Received(1)
            .RollbackAsync(actionContext.HttpContext.RequestAborted);
    }

    [Fact]
    public async Task It_should_when_unhandled_exception()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );

        var endpointMetadata = new List<object> { new TransactionAttribute() };

        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>()!,
            Substitute.For<Controller>()
        )
        {
            Result = null
        };
        var newEndpointMetadata = new List<object>(actionExecutingContext.ActionDescriptor.EndpointMetadata);
        newEndpointMetadata.AddRange(endpointMetadata);

        actionExecutingContext.ActionDescriptor.EndpointMetadata = newEndpointMetadata;

        _notificationManager.HasNotifications.Returns(true);

        var next = Substitute.For<ActionExecutionDelegate>();

        next
            .Invoke()
            .Returns(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Substitute.For<Controller>())
            {
                Exception = new Exception("Test exception"),
                ExceptionHandled = false
            });

        // Act
        await _transactionFilter.OnActionExecutionAsync(actionExecutingContext, next);

        // Assert
        await _unitOfWork
            .Received(1)
            .BeginTransactionAsync(actionContext.HttpContext.RequestAborted);
        await _unitOfWork
            .Received(1)
            .RollbackAsync(actionContext.HttpContext.RequestAborted);
    }

    [Fact]
    public async Task It_should_not_commit_when_transaction_attribute_does_not_exist()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );

        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>()!,
            Substitute.For<Controller>()
        )
        {
            Result = null
        };
        var newEndpointMetadata = new List<object>(actionExecutingContext.ActionDescriptor.EndpointMetadata);

        actionExecutingContext.ActionDescriptor.EndpointMetadata = newEndpointMetadata;

        var next = Substitute.For<ActionExecutionDelegate>();

        // Act
        await _transactionFilter.OnActionExecutionAsync(actionExecutingContext, next);

        // Assert
        await next
            .Received(1)();
        await _unitOfWork
            .DidNotReceiveWithAnyArgs()
            .BeginTransactionAsync();
        await _unitOfWork
            .DidNotReceiveWithAnyArgs()
            .RollbackAsync();
        await _unitOfWork
            .DidNotReceiveWithAnyArgs()
            .CommitAsync();
    }
}