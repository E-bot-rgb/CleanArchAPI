namespace CleanArchAPI.Application.Common.DTOs;

public record CategoryDto(int Id, string Name, string? Description, int ProductCount);
public record CreateCategoryDto(string Name, string? Description);
public record UpdateCategoryDto(string Name, string? Description);