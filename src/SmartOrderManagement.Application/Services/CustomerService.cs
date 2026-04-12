using AutoMapper;
using FluentValidation;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ICreateCustomerValidator _createCustomerValidator;
        private readonly IUpdateCustomerValidator _updateCustomerValidator;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ICreateCustomerValidator createCustomerValidator, IUpdateCustomerValidator updateCustomerValidator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _createCustomerValidator = createCustomerValidator;
            _updateCustomerValidator = updateCustomerValidator;
        }

        /*public async Task<int> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var result= await _createCustomerValidator.ValidateAsync(createCustomerDto);
            if (!result.IsValid)
            {
                throw new ValidationMyException("Validator hatası.");
            }
            var value = _mapper.Map<Customer>(createCustomerDto);
            await _customerRepository.AddAsync(value);
            return value.CustomerId;
            
        }*/

        /*public async Task DeleteCustomerAsync(int id)
        {
            if(id <= 0)
            {
                throw new NotFoundException("Lütfen pozitif bir Id giriniz.");
            }
            var value=await _customerRepository.GetByIdAsync(id);
            if(value is null)
            {
                throw new NotFoundException("Girilen Id'ye ait kullanıcı bulunamadı");
            }
            await _customerRepository.DeleteAsync(value);
        }*/

        public async Task<CustomerByIdDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotFoundException("Lütfen pozitif bir Id giriniz.");
            }
            var value = await _customerRepository.GetByIdAsync(id);
            if (value is null)
            {
                throw new NotFoundException("Girilen Id'ye ait kullanıcı bulunamadı");
            }
            return _mapper.Map<CustomerByIdDto>(value);
        }

        /*public async Task<List<CustomerListDto>> GetCustomersAsync()
        {
            var values=await _customerRepository.GetAllAsync();
            return _mapper.Map<List<CustomerListDto>>(values);
        }*/

        /*public async Task UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
        {
            if (id == updateCustomerDto.CustomerId)
            {
                throw new ValidationMyException("Id'ler uyuşmuyor");
            }
            var result = await _updateCustomerValidator.ValidateAsync(updateCustomerDto);
            if (!result.IsValid)
            {
                throw new ValidationMyException("Validasyon Hatası");
            }
            var value=_mapper.Map<Customer>(updateCustomerDto);
            await _customerRepository.UpdateAsync(value);
        }*/
    }
}
