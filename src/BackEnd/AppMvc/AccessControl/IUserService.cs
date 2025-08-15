using Business.ViewModels;

namespace AppMvc.Interfaces;

public interface IUserService
{
    Task<Guid?> RegisterAsync(UserViewModel userViewModel, string roleName, CancellationToken cancellationToken);

    Task<Guid?> LoginAsync(UserViewModel userViewModel, CancellationToken cancellationToken);
}
