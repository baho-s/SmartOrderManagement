using AutoMapper;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            //CreateMap<src(kaynak nesne), dest(hedef nesne)>();
            //kaynak nesne nedir? Hangi nesneden veri alacağız?
            //hedef nesne nedir? Hangi nesneye veri aktaracağız?

            CreateMap<CreateProductDto, Product>();

            CreateMap<Product, ProductListDto>()
                .ForMember(dest=>dest.CategoryName,
                opt=>opt.MapFrom(src=>src.Category.CategoryName));
                
            CreateMap<Product, ProductByIdDto>();

            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts=>opts.Condition((src,dest,srcMember)=>srcMember!=null));
            //ForAllMembers kullanarak null olmayan değerleri güncelleme işlemi yapıyoruz.
            //src => kaynak nesne (UpdateProductDto),dest => hedef nesne (Product),
            //srcMember => kaynak nesnenin üyesi (UpdateProductDto'daki özellikler)

        }
    }
}
