using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Features.Categories.Command.CreateCategory;
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
        private readonly ICategoryService _categoryService;
        private readonly IMediator _mediator;


        public CategoriesController(ICategoryService categoryService, IMediator mediator)
        {
            _categoryService = categoryService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Created($"api/categories/{command.CategoryName}", result);

        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id,bool AltKategoriSil,int? newParentId)
        {
            await _categoryService.DeleteCategoryAsync(id,AltKategoriSil,newParentId);
            return NoContent(); //204
        }*/

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            // Service async olduğu için await kullanıyoruz.
            var values = await _categoryService.GetCategoriesAsync();

            // 200 OK ile DTO listesini kullanıcıya döndürüyoruz.
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query= new GetCategoryByIdQuery { CategoryId = id };
            var value = await _mediator.Send(query);   
            return Ok(value);
        }

        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            return Ok(ApiResponse<UpdateCategoryDto>.Succes(updateCategoryDto, "Güncelleme Başarılı"));
        }*/

        //GET → [HttpGet("{id}")]
        //PUT → [HttpPut("{id}")]
        //URL resource'ı temsil eder
        //Method adı action'ı temsil eder

        [HttpGet("QueryGet")]
        public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoryQueryDto queryDto)//FromQuery Json formatı yerine
                                                                                                   //Api denerken textbox formatı sunuyor.
        {
            var values = await _categoryService.GetAllAsync(queryDto);
            // Query parametreleri otomatik DTO'ya bind edilir
            // Örnek:
            // /api/categories?page=2&pageSize=5

            return Ok(values);
            // Sonuç client'a döndürülür
        }

        /*[HttpGet("tree")] //SERVİCE İLE ÇALIŞAN
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            return Ok(result);
        }*/

        [HttpPatch("{id}/categoryname")]
        public async Task<IActionResult> UpdateCategoryNameAsync(int id,UpdateCategoryNameAndDescriptionCommand command)
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
    }
}
