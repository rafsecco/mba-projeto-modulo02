using AppMvc.Interfaces;
using Business.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AppMvc.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Guid?> RegisterAsync(UserViewModel userViewModel, string roleName, CancellationToken cancellationToken)
    {
        var identityUser = new IdentityUser
        {
            Email = userViewModel.Email,
            UserName = userViewModel.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, userViewModel.Password);
        if (result.Succeeded)
        {
            await AddRoleToUser(identityUser, roleName);
            return Guid.Parse(identityUser.Id);
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

    private async Task AddRoleToUser(IdentityUser user, string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            await _userManager.AddToRoleAsync(user, roleName); 
        }
    }
}
