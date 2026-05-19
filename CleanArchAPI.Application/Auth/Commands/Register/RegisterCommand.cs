using CleanArchAPI.Application.Auth.Interfaces;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CleanArchAPI.Application.Auth.Commands.Register;

public record RegisterCommand(string Username, string Password, string Role = "User")
    : IRequest<AuthResponseDto>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).Must(r => r == "Admin" || r == "User")
            .WithMessage("Role must be 'Admin' or 'User'.");
    }
}

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtService _jwt;

    public RegisterHandler(IUserRepository users, IJwtService jwt)
        => (_users, _jwt) = (users, jwt);

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = new AppUser
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };
        var created = await _users.CreateAsync(user);
        return new AuthResponseDto(_jwt.GenerateToken(created), created.Username, created.Role);
    }
}