using CleanArchAPI.Application.Categories.Commands.CreateCategory;
using CleanArchAPI.Application.Categories.Commands.DeleteCategory;
using CleanArchAPI.Application.Categories.Queries.GetAllCategories;
using CleanArchAPI.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllCategoriesQuery()));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(dto.Name, dto.Description));
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteCategoryCommand(id));
        return success ? NoContent() : NotFound();
    }
}