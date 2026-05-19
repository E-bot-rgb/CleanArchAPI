namespace CleanArchAPI.Application.Common.DTOs;

public record ProductDto(int Id, string Name, string? Description,
    decimal Price, int Stock, int CategoryId, string CategoryName);

public record CreateProductDto(string Name, string? Description,
    decimal Price, int Stock, int CategoryId);

public record UpdateProductDto(string Name, string? Description,
    decimal Price, int Stock, int CategoryId);