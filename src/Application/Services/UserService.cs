using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IVendedorRepository _vendedorRepository;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IVendedorRepository vendedorRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _vendedorRepository = vendedorRepository;
    }

    public async Task<Guid?> RegisterAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var identityUser = new IdentityUser
        {
            Email = userViewModel.Email,
            UserName = userViewModel.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, userViewModel.Password);
        if (result.Succeeded)
        {
            var vendedor = new Vendedor
            {
                UserId = Guid.Parse(identityUser.Id)
            };
            return await _vendedorRepository.CreateAsync(vendedor, cancellationToken);
        }
        return null;
    }

    public async Task<Guid?> LoginAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(userViewModel.Email, userViewModel.Password, false, true);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(userViewModel.Email);
            if (user is not null)
            {
                return Guid.Parse(user.Id);
            }
        }
        return null;
    }
}

public interface IUserService
{
    Task<Guid?> RegisterAsync(UserViewModel userViewModel, CancellationToken cancellationToken);

    Task<Guid?> LoginAsync(UserViewModel userViewModel, CancellationToken cancellationToken);
}