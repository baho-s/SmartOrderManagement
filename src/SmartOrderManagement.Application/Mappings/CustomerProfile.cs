using AutoMapper;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            //CreateMap<src(kaynak nesne), dest(hedef nesne)>();
            //kaynak nesne nedir? Hangi nesneden veri alacağız?
            //hedef nesne nedir? Hangi nesneye veri aktaracağız?

            CreateMap<CreateCustomerDto, Customer>();

            CreateMap<UpdateCustomerDto, Customer>();

            CreateMap<Customer, CustomerListDto>();

            CreateMap<Customer, CustomerByIdDto>();
        }
    }
}
