namespace ProjectK.Api.UseCases.v1.Users.Interfaces;

public interface IDeleteUserUseCase
{
    Task ExecuteAsync(Ulid userId, CancellationToken cancellationToken = default);
}