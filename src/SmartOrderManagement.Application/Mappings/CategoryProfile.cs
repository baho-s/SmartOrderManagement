using AutoMapper;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryListDto>();

            CreateMap<Category, CategoryDto>();

            CreateMap<Category, CategoryByIdDto>();


            // DTO → Entity (yazma işlemleri)
            // CreateCategoryDto gelir, Category entity'sine dönüşür
            CreateMap<CreateCategoryDto, Category>().ReverseMap();

            // 2A. Entity -> DTO (GET için)
            // Veritabanından gelen Category'yi UpdateCategoryDto'ya çevir
            // Güncelleme formunu doldurmak için kullanılır
            CreateMap<Category, UpdateCategoryDto>()
                // CategoryId'yi map'le (ignore etme çünkü hangi kategoriyi güncelleyeceğimizi bilmeliyiz)
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.CategoryId))

                // CategoryName'i map'le
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.CategoryName))

                // CategoryDescription'ı map'le
                .ForMember(dest => dest.CategoryDescription,
                    opt => opt.MapFrom(src => src.CategoryDescription))

                // ImageUrl'yi map'le (nullable)
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ImageUrl))

                // ParentCategoryId'yi map'le (nullable - root kategoriler için null)
                .ForMember(dest => dest.ParentCategoryId,
                    opt => opt.MapFrom(src => src.ParentCategoryId));

            // 2B. DTO -> Entity (PUT için) - ÖNEMLİ!
            // Kullanıcıdan gelen güncelleme verisini Entity'ye aktar
            //CreateMap<UpdateCategoryDto, Category>()
            //    // CategoryId'yi map'le (hangi kaydı güncelleyeceğimizi bilmek için)
            //    .ForMember(dest => dest.CategoryId,
            //        opt => opt.MapFrom(src => src.CategoryId))

            //    // CategoryName'i güncelle
            //    .ForMember(dest => dest.CategoryName,
            //        opt => opt.MapFrom(src => src.CategoryName))

            //    // CategoryDescription'ı güncelle
            //    .ForMember(dest => dest.CategoryDescription,
            //        opt => opt.MapFrom(src => src.CategoryDescription))

            //    // ImageUrl'yi güncelle
            //    .ForMember(dest => dest.ImageUrl,
            //        opt => opt.MapFrom(src => src.ImageUrl))

            //    // *** ÖNEMLİ: ParentCategoryId'yi güncelle ***
            //    // Null gelebilir (root kategori için) - bu yüzden MapFrom kullan
            //    .ForMember(dest => dest.ParentCategoryId,
            //        opt => opt.MapFrom(src => src.ParentCategoryId))

            //    // *** Navigation Property'leri IGNORE ET ***
            //    // ParentCategory: EF Core'un kendi yükler, bizim doldurmamalıyız
            //    // Sadece FK (ParentCategoryId) ile ilişki kurulur
            //    .ForMember(dest => dest.ParentCategory,
            //        opt => opt.Ignore())

            //    // SubCategories: Mevcut alt kategoriler korunur
            //    // Güncelleme sırasında alt kategorileri silmek istemeyiz
            //    .ForMember(dest => dest.SubCategories,
            //        opt => opt.Ignore())

            //    // Products: Mevcut ürünler korunur
            //    // Kategori güncellerken ürünleri etkilemek istemeyiz
            //    .ForMember(dest => dest.Products,
            //        opt => opt.Ignore())

            //    // *** BaseEntity Property'lerini IGNORE ET ***
            //    // CreatedDate: İlk oluşturulma tarihi değişmez
            //    .ForMember(dest => dest.CreatedDate,
            //        opt => opt.Ignore())

            //    // UpdatedDate: Service katmanında manuel set edilir (DateTime.Now)
            //    .ForMember(dest => dest.UpdatedDate,
            //        opt => opt.Ignore())

            //    // IsDeleted: Update sırasında değişmez (ayrı Delete metodu var)
            //    .ForMember(dest => dest.IsDeleted,
            //        opt => opt.Ignore());

            CreateMap<UpdateCategoryDto, Category>()//Ignore şu anlama geliyor. Bu alanları mapping yapma güncellenmeyecek.
                                                    //Eski değerler olduğu gibi gelecek ağacın devamı varsa kaybetmemek için.
    .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())//ParentCategory güncelleme sırasında korunacak, silinmeyecek
    .ForMember(dest => dest.SubCategories, opt => opt.Ignore())//SubCategories güncelleme sırasında korunacak, silinmeyecek
    .ForMember(dest => dest.Products, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
    .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());



            // Recursive mapping - alt kategoriler otomatik dönüşür
            CreateMap<Category, CategoryTreeDto>()//Category entity'sini CategoryTreeDto'ya çevirirken özel mapping yapıyoruz.
                .ForMember(dest => dest.ParentCategoryName,//destination tarafında ParentCategoryName var, source tarafında ParentCategory var. ParentCategory null değilse ParentCategory.CategoryName'i map'le, null ise null bırak.
                opt => opt.MapFrom(src => src.ParentCategory != null
                ? src.ParentCategory.CategoryName : null))
                .ForMember(dest => dest.ProductCount,//ürün sayısını map'le (Products koleksiyonunun sayısı)
                opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.SubCategories,//alt kategorileri map'le (recursive mapping)
                opt => opt.MapFrom(src => src.SubCategories));
            //Mantığını anlatayım. Category entity'sinin SubCategories özelliği var.
            //Bu özellik, alt kategorilerin listesi. AutoMapper, bu alt kategorileri de CategoryTreeDto'ya
            //çevirecek çünkü aynı mapping tanımlı. Yani her alt kategori de kendi alt kategorilerine sahip ola
            //bilir ve onlar da otomatik olarak DTO'ya dönüşür. Bu sayede tüm kategori ağacını
            //tek seferde DTO'ya çevirebiliriz.

        }
    }
}
