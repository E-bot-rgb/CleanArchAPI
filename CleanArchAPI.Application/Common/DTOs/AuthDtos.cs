namespace CleanArchAPI.Application.Common.DTOs;

public record RegisterDto(string Username, string Password, string Role = "User");
public record LoginDto(string Username, string Password);
public record AuthResponseDto(string Token, string Username, string Role);