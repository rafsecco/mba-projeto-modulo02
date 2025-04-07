using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.App.User.Commands;

public class LoginCommand : IRequest<Guid?>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Guid?>
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginCommandHandler(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Guid?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                return Guid.Parse(user.Id);
            }
        }
        return null;
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty();
    }
}