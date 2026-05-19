using CleanArchAPI.Application.Auth.Commands.Login;
using CleanArchAPI.Application.Auth.Commands.Register;
using CleanArchAPI.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>Registrera ny användare och få JWT-token.</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        => Ok(await _mediator.Send(new RegisterCommand(dto.Username, dto.Password, dto.Role)));

    /// <summary>Logga in och få JWT-token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginCommand(dto.Username, dto.Password));
        return result is null ? Unauthorized(new { message = "Invalid credentials." }) : Ok(result);
    }
}