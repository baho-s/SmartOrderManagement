# 📦 SmartOrderManagement

Layered architecture, SOLID prensipleri ve .NET backend geliştirmeyi öğrenmek için oluşturulan **Sipariş Yönetim Sistemi** projesi. Kategori, Ürün, Müşteri ve Sipariş yönetiminin tam işlevli e-ticaret API'si. **JWT Authentication** ve **MediatR Pattern** ile geliştirilmiş modern .NET API.

## 🎯 Öğrenme Hedefleri

- ✅ Layered Architecture (4-tier)
- ✅ SOLID Prensipleri (SRP, OCP, LSP, ISP, DIP)
- ✅ Dependency Injection & IoC Container
- ✅ Entity Framework Core & Migrations
- ✅ FluentValidation & AutoMapper
- ✅ Repository Pattern & Service Layer
- ✅ Exception Handling Middleware
- ✅ RESTful API Design
- ✅ Unit Testing (NUnit)
- ✅ Soft Delete Pattern
- ✅ **JWT Authentication & Authorization**
- ✅ **MediatR CQRS Pattern (Commands & Queries)**
- ✅ **Unit of Work Pattern**

## 🏗️ Proje Mimarisi

```
SmartOrderManagement
├── SmartOrderManagement.API              → Presentation (Controllers, Middlewares)
├── SmartOrderManagement.Application      → Business Logic (Features/Commands/Queries, Validators, DTOs)
├── SmartOrderManagement.Domain           → Entities & Business Rules
├── SmartOrderManagement.Infrastructure   → Data Access (Repositories, DbContext, UnitOfWork)
└── SmartOrderManagementNUnitTest         → Unit Tests
```

**Veri Akışı:**
```
HTTP Request → [API Layer] → [Application - MediatR] → [Infrastructure] → [Domain/DB]
```

| Katman | Sorumluluk |
|--------|-----------|
| **API** | Controllers, Middlewares, Exception Handling, JWT Validation |
| **Application** | Features (Commands/Queries), DTOs, Validators, Mappings |
| **Domain** | Entities, Business Rules, Enums |
| **Infrastructure** | Repositories, DbContext, Migrations, UnitOfWork, Auth Services |

## 🔧 Teknolojiler

| Teknoloji | Kullanım |
|-----------|---------|
| **ASP.NET Core 8.0** | Web API Framework |
| **Entity Framework Core** | ORM |
| **SQL Server** | Database |
| **FluentValidation** | Validasyon |
| **AutoMapper** | DTO Mapping |
| **NUnit** | Unit Testing |
| **MediatR** | CQRS Pattern |
| **JWT (JSON Web Token)** | Authentication |
| **Swagger/OpenAPI** | API Documentation & JWT Support |

## 📋 SOLID Prensipleri

**Single Responsibility (SRP):** Her Command Handler kendi işini yapıyor, Validator sadece doğrulama, Repository sadece DB işlemleri

**Open/Closed (OCP):** Interface ile genişlemeye açık - yeni Command ekle, mevcut kod değişmez

**Liskov Substitution (LSP):** Her repository Interface'i aynı şekilde implement eder

**Interface Segregation (ISP):** Validator'ler ayrı interface'lere bölünmüş

**Dependency Inversion (DIP):** Concrete class'a değil, interface'e bağımlı - DI Container register eder

## 📁 Proje Yapısı

**API Layer:** Controllers, ExceptionMiddleware, JWT Token Validation

**Application Layer:**
- Features/
  - Auth/ → Register, Login Commands
  - Products/, Categories/, Customers/, Orders/ → Create, Update, Delete Commands & Read Queries
- DTOs/ → CreateCategoryDto, UpdateCategoryDto, CategoryListDto vb.
- Validators/ → CreateCategoryValidator, UpdateCategoryValidator vb.
- Interfaces/ → IService, IRepository, IValidator, IUnitOfWork abstractions
- Mappings/ → AutoMapper Profiles (Entity ↔ DTO)
- Exceptions/ → Custom exception sınıfları

**Domain Layer:**
- Entities/ → Category, Product, Order, OrderItem, Customer, **AppUser** (yeni)
- BaseEntity/ → CreatedDate, UpdatedDate, IsDeleted (Soft Delete)
- Enums/ → OrderStatus

**Infrastructure Layer:**
- Context/ → AppDbContext
- Repositories/ → CategoryRepository, ProductRepository, OrderRepository, **AppUserRepository** (yeni)
- Migrations/ → Database schema versioning
- UnitOfWork/ → **IUnitOfWork, UnitOfWork pattern** (yeni)
- Services/ → **PasswordHasher, JwtService** (yeni - Authentication)

## 🚀 Başlangıç

### Gereksinimler
- .NET 8.0 SDK
- SQL Server

### Kurulum

```bash
# 1. Repository'i klonla
git clone <repository-url>

# 2. appsettings.json'da bağlantı stringini ve JWT ayarlarını düzenle
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=SmartOrderManagement;Trusted_Connection=true;"
},
"Jwt": {
  "Key": "SmartOrderManagement_SuperSecretKey_2024!",
  "Issuer": "SmartOrderManagement.API",
  "Audience": "SmartOrderManagement.Client"
}

# 3. Migrate
dotnet ef database update --project src/SmartOrderManagement.Infrastructure

# 4. Çalıştır
dotnet run --project src/SmartOrderManagement.API
```

## 📡 API Endpoints

| Kaynak | Endpoints |
|--------|-----------|
| **Auth** | `POST /api/auth/register`, `POST /api/auth/login` |
| **Kategoriler** | `POST/GET/PUT/DELETE /api/categories` |
| **Ürünler** | `POST/GET/PUT/DELETE /api/products` |
| **Müşteriler** | `POST/GET/PUT/DELETE /api/customers` |
| **Siparişler** | `POST/GET/PUT/DELETE /api/orders` |
| **Sipariş Ürünleri** | `POST/GET/PUT/DELETE /api/orderitems` |

**Auth Örneği:**
```http
POST /api/auth/register
{ "username": "user@example.com", "password": "Pass123!" }
→ 201 Created, JWT Token döner

POST /api/auth/login
{ "username": "user@example.com", "password": "Pass123!" }
→ 200 OK, JWT Token döner

GET /api/categories
Authorization: Bearer <JWT_TOKEN>
→ 200 OK
```

**Kategori Örneği:**
```http
POST /api/categories
Authorization: Bearer <JWT_TOKEN>
{ "categoryName": "Elektronik", "categoryDescription": "..." }
→ 201 Created

GET /api/categories → 200 OK
DELETE /api/categories/1 → 204 No Content
```

## 🧪 Unit Testing

```bash
dotnet test SmartOrderManagementNUnitTest.csproj
```

## 🎓 Uygulanmış Konseptler

**Architectural Patterns:**
- Layered Architecture (N-Tier)
- **MediatR CQRS Pattern** (Commands & Queries)
- **Unit of Work Pattern**
- Dependency Injection
- DTO Pattern
- AutoMapper Pattern
- Middleware Pattern
- **JWT Authentication Pattern**

**Best Practices:**
- Fluent Validation (RuleFor, NotEmpty, MinimumLength, MaximumLength)
- Global Exception Handling (merkezi middleware)
- Soft Delete (IsDeleted flag)
- Entity Relationships (Navigation Properties, Foreign Keys)
- Async/Await (Tüm DB işlemleri)
- Entity ↔ DTO Mapping
- **Secure Password Hashing**
- **JWT Token Generation & Validation**

## 📊 Veri Modeli

**Entities:**
- Category (1) ↔ (N) Product
- Customer (1) ↔ (N) Order
- Order (1) ↔ (N) OrderItem
- Product (1) ↔ (N) OrderItem
- **AppUser (1) ↔ (N) Order** (yeni - Authentication)
- BaseEntity: CreatedDate, UpdatedDate, IsDeleted

**Veri Akışı (Create Product - MediatR Pattern):**
```
POST Request → Controller 
→ CreateProductCommand Dispatch (MediatR)
→ CreateProductCommandHandler (Business Logic)
→ Service/Validation 
→ UnitOfWork.ProductRepository 
→ Database 
→ Mapper: DTO'ya dönüştür 
→ 201 Response
```

## 🔐 Exception Handling

Merkezi ExceptionMiddleware tüm hataları yakalar ve uygun HTTP statüsü döner:

- ValidationMyException → 400 Bad Request (validasyon kuralı ihlali)
- NotFoundException → 404 Not Found (kayıt bulunamadı)
- BusinessRuleException → 409 Conflict (iş kuralı ihlali)
- Exception → 500 Internal Server Error

## 🔐 JWT Authentication

**AppUser Entity:**
- Username, PasswordHash, Email
- Register & Login işlemleri

**Authentication Flow:**
1. Kullanıcı Register olur → PasswordHasher ile şifre hashlanır
2. Kullanıcı Login olur → Şifre doğrulanır
3. JwtService JWT Token oluşturur (Issuer, Audience, Expiration ile)
4. Client Token'ı Header'da Bearer şeması ile gönderir
5. API Token'ı doğrular ve request'i işler

**Swagger JWT Desteği:**
- Swagger UI'da "Authorize" button ile JWT Token girebilirsiniz
- Token'ı yapıştırdıktan sonra bütün endpoint'ler otomatik olarak Authorization header'ı kullanır

## 🤝 Yenilikleri (Eski Sürüm vs Güncel)

- ✨ **MediatR CQRS Pattern**: Eski Services yerine Features/Commands/Queries
- ✨ **JWT Authentication**: Register/Login ve Token-based authorization
- ✨ **Unit of Work Pattern**: Transaksiyon yönetimi iyileştirildi
- ✨ **AppUser Entity**: Kullanıcı yönetimi ve authentication
- ✨ **Swagger JWT Support**: API documantasyonda JWT token desteği
- ✨ **OrderValidator**: Order validation kuralları eklendi

## 🤝 Gelecek Geliştirmeler

- ✅ **JWT Authentication** (Eklendi)
- [ ] API Versioning
- ✅ Caching 
- [ ] Comprehensive Unit Tests
- [ ] Integration Tests
- ✅ **Swagger/OpenAPI Dokumentasyonu** (Eklendi - JWT Support ile)
- ✅ Logging

---

**Not:** Bu proje professional backend mimarisi, SOLID prensipleri, modern .NET patterns ve JWT authentication'ın pratik uygulamasını göstermektedir.
