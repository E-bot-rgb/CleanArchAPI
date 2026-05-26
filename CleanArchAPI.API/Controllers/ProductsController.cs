using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Application.Products.Commands.CreateProduct;
using CleanArchAPI.Application.Products.Commands.DeleteProduct;
using CleanArchAPI.Application.Products.Commands.UpdateProduct;
using CleanArchAPI.Application.Products.Queries.GetAllProducts;
using CleanArchAPI.Application.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Hämta produkter med valfria filter och paginering.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ProductDto>), 200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(
            name, categoryId, minPrice, maxPrice, page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(
            new CreateProductCommand(dto.Name, dto.Description, dto.Price, dto.Stock, dto.CategoryId));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var result = await _mediator.Send(
            new UpdateProductCommand(id, dto.Name, dto.Description, dto.Price, dto.Stock, dto.CategoryId));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteProductCommand(id));
        return success ? NoContent() : NotFound();
    }
}