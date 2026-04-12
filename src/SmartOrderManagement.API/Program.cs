using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartOrderManagement.API.Middlewares;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Mappings;
using SmartOrderManagement.Application.Services;
using SmartOrderManagement.Application.Validators.CategoryValidators;
using SmartOrderManagement.Infrastructure.Context;
using SmartOrderManagement.Infrastructure.Repositories;
using System.Reflection;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Application.Validators.ProductValidators;
using SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators;
using SmartOrderManagement.Application.Validators.CustomerValidators;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Infrastructure.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.OrderValidators;
using SmartOrderManagement.Application.Validators.OrderValidators;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderList;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderById;
using SmartOrderManagement.Application.Features.Orders.Command.CreateOrder;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderStatus;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderAddress;
using SmartOrderManagement.Application.Features.Orders.Command.DeleteOrder;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderTotalAmount;
using SmartOrderManagement.Application.Features.Products.Command.CreateProduct;
using SmartOrderManagement.Application.Features.Products.Command.DeleteProduct;
using SmartOrderManagement.Application.Features.Products.Command.UpdateProductIsActive;
using SmartOrderManagement.Application.Features.Products.Command.UpdateProductCategoryId;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(CategoryProfile).Assembly);  // Profile'ın olduğu assembly
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
//“Bu interface istendiğinde, şu class’ı ver” dedik.

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();

builder.Services.AddScoped<ICreateCategoryValidator, CreateCategoryValidator>();
builder.Services.AddScoped<IUpdateCategoryValidator, UpdateCategoryValidator>();

builder.Services.AddScoped<ICreateProductValidator,CreateProductValidator>();
builder.Services.AddScoped<IUpdateProductValidator,UpdateProductValidator>();

builder.Services.AddScoped<ICreateCustomerValidator, CreateCustomerValidator>();
builder.Services.AddScoped<IUpdateCustomerValidator,UpdateCustomerValidator>();

builder.Services.AddScoped<ICreateOrderValidator, CreateOrderValidator>();

// Order ile ilgili command ve query handler'ları DI container'a ekliyoruz
builder.Services.AddScoped<CreateOrderCommandHandler>();
builder.Services.AddScoped<UpdateOrderStatusCommandHandler>();
builder.Services.AddScoped<GetOrderByIdQueryHandler>();
builder.Services.AddScoped<GetOrdersListQueryHandler>();
builder.Services.AddScoped<UpdateOrderAddressCommandHandler>();
builder.Services.AddScoped<UpdateOrderTotalAmountCommandHandler>();
builder.Services.AddScoped<DeleteOrderCommandHandler>();

//Product ile ilgili command handler'ları DI container'a ekliyoruz
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
// Application katmanındaki validator'ları otomatik bulup DI container'a ekler

builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  YENİ: Middleware HER ZAMAN ekle (Development ve Production'da)
//app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ExceptionMiddleware>();
// Uygulamaya kendi yazdığımız exception middleware'ini ekliyoruz
//
// Bu satırdan sonra request geldiğinde,
// önce bu middleware devreye girer
//
// Eğer aşağı tarafta (controller, service, repository)
// bir exception fırlatılırsa,
// bu middleware onu yakalar ve JSON response döner

// Şimdilik kapalı kalsın
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();