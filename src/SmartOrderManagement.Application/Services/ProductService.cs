using AutoMapper;
using FluentValidation;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICreateProductValidator _createProductValidator;
        private readonly IUpdateProductValidator _updateProductValidator;

        public ProductService(IProductRepository productRepository, IMapper mapper, ICreateProductValidator createProductValidator, IUpdateProductValidator updateProductValidator, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _createProductValidator = createProductValidator;
            _updateProductValidator = updateProductValidator;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreateProductAsync(CreateProductDto createProductDto)
        {

            var result=await _createProductValidator.ValidateAsync(createProductDto);
            if (!result.IsValid)
            {
                throw new ValidationMyException("Validasyon Hatası");
            }

            if(!await _categoryRepository.ExistsAsync(createProductDto.CategoryId))
            {
                throw new BusinessRuleException("Kategori bulunamadı.");
            }

            var value=_mapper.Map<Product>(createProductDto);
            //createProductDto nesnesini Product nesnesine dönüştürür ve value değişkenine atar.
            await _productRepository.AddAsync(value);
            return value.ProductId;
        }

        public async Task DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotFoundException("Lütfen pozitif bir Id giriniz.");
            }
            var value=await _productRepository.GetByIdAsync(id);
            if(value is null)
            {
                throw new NotFoundException("Bu Id'ye ait ürün bulunamadı.");
            }

            await _productRepository.DeleteAsync(value);
        }

        public async Task<ProductByIdDto> GetByIdAsync(int id)
        {
            var value=await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductByIdDto>(value);
        }

        public async Task<List<ProductListDto>> GetProductsAsync()
        {
            var values=await _productRepository.GetProductsAsync();
            return _mapper.Map<List<ProductListDto>>(values);

        }

        public async Task UpdateProductAsync(int id,UpdateProductDto updateProductDto)
        {
            if (id != updateProductDto.ProductId)
            {
                throw new BusinessRuleException("ID'ler eşleşmiyor.");
            }
            var value=_mapper.Map<Product>(updateProductDto);
            await _productRepository.UpdateAsync(value);
        }
    }
}
