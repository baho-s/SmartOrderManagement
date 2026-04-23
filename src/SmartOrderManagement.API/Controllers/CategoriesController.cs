using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Features.Categories.Command.CreateCategory;
using SmartOrderManagement.Application.Features.Categories.Command.DeleteCategory;
using SmartOrderManagement.Application.Features.Categories.Command.DeleteCategoryMoveChildren;
using SmartOrderManagement.Application.Features.Categories.Command.UpdateCategory;
using SmartOrderManagement.Application.Features.Categories.Command.UpdateCategoryParentCategory;
using SmartOrderManagement.Application.Features.Categories.Query.GetCategoryById;
using SmartOrderManagement.Application.Features.Categories.Query.GetCategoryListTree;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;


        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Created($"api/categories/{command.CategoryName}", result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery { CategoryId = id };
            var value = await _mediator.Send(query);
            return Ok(value);
        }

        [HttpPatch("{id}/categoryname")]
        public async Task<IActionResult> UpdateCategoryNameAsync(int id, UpdateCategoryNameAndDescriptionCommand command)
        {
            command.CategoryId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/parentcategory")]
        public async Task<IActionResult> UpdateCategoryParentCategoryAsync(int id, UpdateCategoryParentCategoryCommand command)
        {
            command.CategoryId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetCategoryTree([FromQuery] GetCategoryListTreeQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryItsChildren(int id)
        {
            var command = new DeleteCategoryItsChildrenCommand { CategoryId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/move-children")]
        public async Task<IActionResult> DeleteCategoryMoveChildren(int id, [FromQuery] int newParentCategoryId)
        {
            var command = new DeleteCategoryMoveChildrenCommand { CategoryId = id, NewParentCategoryId = newParentCategoryId };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
