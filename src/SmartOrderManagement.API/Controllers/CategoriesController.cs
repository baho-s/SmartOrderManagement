using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var result= await _categoryService.CreateCategoryAsync(createCategoryDto);
            return Created($"api/categories/{createCategoryDto.CategoryName}",result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id,bool AltKategoriSil,int? newParentId)
        {
            await _categoryService.DeleteCategoryAsync(id,AltKategoriSil,newParentId);
            return NoContent(); //204
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            // Service async olduğu için await kullanıyoruz.
            var values = await _categoryService.GetCategoriesAsync();

            // 200 OK ile DTO listesini kullanıcıya döndürüyoruz.
            return Ok(values);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var value=await _categoryService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id,UpdateCategoryDto updateCategoryDto)
        {               
            await _categoryService.UpdateCategoryAsync(id,updateCategoryDto);
            return Ok(ApiResponse<UpdateCategoryDto>.Succes(updateCategoryDto,"Güncelleme Başarılı"));
        }

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

        [HttpGet("tree")]
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            return Ok(result);
        }

    }
}
