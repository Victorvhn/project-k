using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectK.Core.Adapters.Driven.Database;
using ProjectK.Database.Contexts;

namespace ProjectK.Database;

[ExcludeFromCodeCoverage]
internal sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext? _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private bool _disposed;

    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_dbContext is null, _dbContext);

        _transaction ??= await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_dbContext is null, _dbContext);
        ObjectDisposedException.ThrowIf(_transaction is null, _transaction);

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_dbContext is null, _dbContext);
        ObjectDisposedException.ThrowIf(_transaction is null, _transaction);

        await _transaction.RollbackAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_dbContext is null, _dbContext);

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
        }

        _disposed = true;
    }

    private async ValueTask DisposeAsyncCore()
    {
        if (_disposed) return;

        if (_transaction is not null) await _transaction.DisposeAsync();

        if (_dbContext is not null) await _dbContext.DisposeAsync();

        _disposed = true;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}