using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Services;
using SmartOrderManagement.Application.Validators.CategoryValidators;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SmartOrderManagementNUnitTest
{
   /* public class Tests
    {
        //UnitOfWork_Condition_ExpectedResult
        [Test]
        public async Task CreateCategoryAsync_WithShortName_ShouldThrowValidationMyException()
        {
            //Bu test CreateCategoryAsync metodunun, kategori adýnýn 3 karakterden kýsa olmasý durumunda ValidationMyException fýrlatýp fýrlatmadýðýný test eder.
            //Ancak baðýmlý bir testtir, CreateCategoryValidator sýnýfýnýn doðru çalýþtýðýný varsayar.
            //Arrange
            //Baðýmlýlýklarý yönetiyoruz.
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<AutoMapper.IMapper>();
            var validatorCreate = new CreateCategoryValidator();
            var validatorUpdateMock = new Mock<FluentValidation.IValidator<UpdateCategoryDto>>();

            //var hata=new ValidationFailure("CategoryName", "Kategori adý en az 3 karakter olmalýdýr.");

            //var hatalistesi=new List<ValidationFailure> { hata };

            //var hataSonucu=new ValidationResult(hatalistesi);

            //validatorCreateMock.Setup(v=>v.ValidateAsync(It.IsAny<CreateCategoryDto>(),It.IsAny<CancellationToken>())).ReturnsAsync(hataSonucu);


            var categoryServiceMock = new CategoryService(categoryRepositoryMock.Object, mapperMock.Object, validatorCreate, validatorUpdateMock.Object);

            var createCategoryDto = new CreateCategoryDto
            {
                CategoryName = "a",
            };

            //Action
            //Async metodlar için Func<Task> kullanýlýr
            Func<Task> action=async () => await categoryServiceMock.CreateCategoryAsync(createCategoryDto);

            //Assert
            //ThrowAsync kullanarak asenkron hatayý bekliyoruz.
            await action.Should().ThrowAsync<ValidationMyException>();
        }

        [Test]
        public async Task CreateCategory_WhenCategoryNameAlreadyExists_ShouldThrowBusinessMyException()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();

            var createCategoryDto=new CreateCategoryDto
            {
                CategoryName = "Elektrik",
                CategoryDescription = "This is a test category",
                ImageUrl = "http://example.com/image.jpg",
                ParentCategoryId = null
            };

            categoryServiceMock.Setup(i=>i.CreateCategoryAsync(createCategoryDto)).
                ThrowsAsync(new BusinessRuleException("Bu kategori zaten mevcut."));

            //Action
            Func<Task> action=async () => await categoryServiceMock.Object.CreateCategoryAsync(createCategoryDto);

            //Assert
            await action.Should().ThrowAsync<BusinessRuleException>();
        }
    }*/
}