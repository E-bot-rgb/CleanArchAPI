using CleanArchAPI.Application.Auth.Interfaces;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<AuthResponseDto?>;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    private readonly IUserRepository _users;
    private readonly IJwtService _jwt;

    public LoginHandler(IUserRepository users, IJwtService jwt)
        => (_users, _jwt) = (users, jwt);

    public async Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _users.GetByUsernameAsync(request.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;
        return new AuthResponseDto(_jwt.GenerateToken(user), user.Username, user.Role);
    }
}