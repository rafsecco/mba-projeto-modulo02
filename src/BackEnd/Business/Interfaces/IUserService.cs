using Business.ViewModels;

namespace Business.Interfaces;

public interface IUserService
{
    Task<Guid?> RegisterAsync(UserViewModel userViewModel, CancellationToken cancellationToken);

    Task<Guid?> LoginAsync(UserViewModel userViewModel, CancellationToken cancellationToken);
}