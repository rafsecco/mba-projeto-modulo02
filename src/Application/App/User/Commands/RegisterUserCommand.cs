using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.App.User.Commands;

public class RegisterUserCommand : IRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterUserCommandHandler(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var identityUser = new IdentityUser
        {
            Email = request.Email,
            UserName = request.UserName,
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (result.Succeeded)
        {
            var seller = new Seller
            {
                UserId = Guid.Parse(identityUser.Id)
            };
            await _dbContext.Sellers.AddAsync(seller, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterUserCommandValidator(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        RuleFor(c => c.UserName).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
        //.MustAsync(EmailValidAsync);
        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("The password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("The password must be at max 100 characters long.");
    }

    private async Task<bool> EmailValidAsync(string email, CancellationToken cancellationToken)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }
}