using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using SmartOrderManagement.Application.Common.Caching;
using SmartOrderManagement.Application.Common.Logging;
using SmartOrderManagement.Application.Features.Auth.Command.Login;
using SmartOrderManagement.Application.Features.Auth.Command.Register;
using SmartOrderManagement.Application.Features.Products.Command.CreateProduct;
using SmartOrderManagement.Application.Interfaces.Caching;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators;
using SmartOrderManagement.Application.Interfaces.Validators.OrderValidators;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Application.Mappings;
using SmartOrderManagement.Application.Services;
using SmartOrderManagement.Application.Validators.CategoryValidators;
using SmartOrderManagement.Application.Validators.CustomerValidators;
using SmartOrderManagement.Application.Validators.OrderValidators;
using SmartOrderManagement.Application.Validators.ProductValidators;
using SmartOrderManagement.Infrastructure.Context;
using SmartOrderManagement.Infrastructure.Repositories;
using SmartOrderManagement.Infrastructure.Services;
using SmartOrderManagement.Infrastructure.UnitOfWork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog yapılandırması
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Genel seviye
    .WriteTo.Console() // Konsol için filtre yok, her şeyi yazar
    .WriteTo.Logger(lc => lc
        // Sadece 'LoggingBehavior' üzerinden gelen logları filtrele
        .Filter.ByIncludingOnly("SourceContext like '%LoggingBehavior%'")
        .WriteTo.File("../../logs/behavior-logs.txt", rollingInterval: RollingInterval.Day)
    )
    .CreateLogger();

builder.Host.UseSerilog(); // Serilog'u uygulama host'una ekle



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(CategoryProfile).Assembly);  // Profile'ın olduğu assembly
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//“Bu interface istendiğinde, şu class’ı ver” dedik.

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();


builder.Services.AddScoped<ICreateCategoryValidator, CreateCategoryValidator>();
builder.Services.AddScoped<IUpdateCategoryValidator, UpdateCategoryValidator>();

builder.Services.AddScoped<ICreateProductValidator,CreateProductValidator>();
builder.Services.AddScoped<IUpdateProductValidator,UpdateProductValidator>();

builder.Services.AddScoped<ICreateCustomerValidator, CreateCustomerValidator>();
builder.Services.AddScoped<IUpdateCustomerValidator,UpdateCustomerValidator>();

builder.Services.AddScoped<ICreateOrderValidator, CreateOrderValidator>();



//MediatR ile bütün implement edilen sınıfların Assembly karşılıkları bulunup Regist ediliyor.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
});//Eğer MediatR kullanmasaydık--> builder.Services.AddScoped<CreateOrderCommandHandler>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Auth ile ilgili servisler
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpContextAccessor();
// IHttpContextAccessor'ı DI container'a tanıttık
// Artık Handler'lara inject edilebilir

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Auth Handler'ları
builder.Services.AddScoped<RegisterCommandHandler>();
builder.Services.AddScoped<LoginCommandHandler>();

// JWT ayarlarını appsettings.json'dan okuyoruz
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Varsayılan authentication şeması JWT olacak
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        // Token'ı kimin oluşturduğunu doğrula

        ValidateAudience = true,
        // Token kimin için oluşturuldu doğrula

        ValidateLifetime = true,
        // Token süresi dolmuş mu doğrula

        ValidateIssuerSigningKey = true,
        // Token imzası geçerli mi doğrula

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        // appsettings.json'daki değerlerle karşılaştır
    };
});



builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
// Application katmanındaki validator'ları otomatik bulup DI container'a ekler

builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartOrderManagement API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Token giriniz. Bearer + Sadece token'ı yapıştırın,",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        // Http yerine ApiKey kullanıyoruz
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Cache için
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddMemoryCache();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
builder.Services.AddScoped<ICacheKeyTracker, CacheKeyTracker>();

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

//Giriş için--------------
app.UseAuthentication();
// Önce Authentication (kim bu kullanıcı?)

app.UseAuthorization();
// Sonra Authorization (bu kullanıcı ne yapabilir?)

app.MapControllers();

app.Run();