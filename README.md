# 📦 SmartOrderManagement

Layered architecture, SOLID prensipleri ve .NET backend geliştirmeyi öğrenmek için oluşturulan **Sipariş Yönetim Sistemi** projesi. Kategori, Ürün, Müşteri ve Sipariş yönetiminin tam işlevli e-ticaret API'si.

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

## 🏗️ Proje Mimarisi

```
SmartOrderManagement
├── SmartOrderManagement.API              → Presentation (Controllers, Middlewares)
├── SmartOrderManagement.Application      → Business Logic (Services,Business Rules Validators, DTOs)
├── SmartOrderManagement.Domain           → Entities 
├── SmartOrderManagement.Infrastructure   → Data Access (Repositories, DbContext)
└── SmartOrderManagementNUnitTest         → Unit Tests
```

**Veri Akışı:**
```
HTTP Request → [API Layer] → [Application] → [Infrastructure] → [Domain/DB]
```

| Katman | Sorumluluk |
|--------|-----------|
| **API** | Controllers, Middlewares, Exception Handling |
| **Application** | Services, DTOs, Validators, Mappings |
| **Domain** | Entities, Enums |
| **Infrastructure** | Repositories, DbContext, Migrations |

## 🔧 Teknolojiler

| Teknoloji | Kullanım |
|-----------|---------|
| **ASP.NET Core 8.0** | Web API Framework |
| **Entity Framework Core** | ORM |
| **SQL Server** | Database |
| **FluentValidation** | Validasyon |
| **AutoMapper** | DTO Mapping |
| **NUnit** | Unit Testing |

## 📋 SOLID Prensipleri

**Single Responsibility (SRP):** Kategori Service sadece kategori işlemleri, Validator sadece doğrulama, Repository sadece DB işlemleri

**Open/Closed (OCP):** Interface ile genişlemeye açık - yeni implementation ekle, mevcut kod değişmez

**Liskov Substitution (LSP):** Her repository Interface'i aynı şekilde implement eder

**Interface Segregation (ISP):** CreateCategoryValidator ve UpdateCategoryValidator ayrı interface'leri var

**Dependency Inversion (DIP):** Concrete class'a değil, interface'e bağımlı - DI Container register eder

## 📁 Proje Yapısı

**API Layer:** Controllers, ExceptionMiddleware (ValidationMyException → 400, NotFoundException → 404, BusinessRuleException → 409)

**Application Layer:**
- Services/ → CategoryService, ProductService, OrderService vb.
- DTOs/ → CreateCategoryDto, UpdateCategoryDto, CategoryListDto vb.
- Validators/ → CreateCategoryValidator, UpdateCategoryValidator vb.
- Interfaces/ → IService, IRepository, IValidator abstractions
- Mappings/ → AutoMapper Profiles (Entity ↔ DTO)
- Exceptions/ → Custom exception sınıfları

**Domain Layer:**
- Entities/ → Category, Product, Order, OrderItem, Customer
- BaseEntity/ → CreatedDate, UpdatedDate, IsDeleted (Soft Delete)
- Enums/ → OrderStatus

**Infrastructure Layer:**
- Context/ → AppDbContext
- Repositories/ → CategoryRepository, ProductRepository, OrderRepository vb.
- Migrations/ → Database schema versioning

## 🚀 Başlangıç

### Gereksinimler
- .NET 8.0 SDK
- SQL Server

### Kurulum

```bash
# 1. Repository'i klonla
git clone <repository-url>

# 2. appsettings.json'da bağlantı stringini ayarla
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=SmartOrderManagement;Trusted_Connection=true;"
}

# 3. Migrate
dotnet ef database update --project src/SmartOrderManagement.Infrastructure

# 4. Çalıştır
dotnet run --project src/SmartOrderManagement.API
```

## 📡 API Endpoints

| Kaynak | Endpoints |
|--------|-----------|
| **Kategoriler** | `POST/GET/PUT/DELETE /api/categories` |
| **Ürünler** | `POST/GET/PUT/DELETE /api/products` |
| **Müşteriler** | `POST/GET/PUT/DELETE /api/customers` |
| **Siparişler** | `POST/GET/PUT/DELETE /api/orders` |
| **Sipariş Ürünleri** | `POST/GET/PUT/DELETE /api/orderitems` |

**Kategori Örneği:**
```http
POST /api/categories
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
- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- DTO Pattern
- AutoMapper Pattern
- Middleware Pattern

**Best Practices:**
- Fluent Validation (RuleFor, NotEmpty, MinimumLength, MaximumLength)
- Global Exception Handling (merkezi middleware)
- Soft Delete (IsDeleted flag)
- Entity Relationships (Navigation Properties, Foreign Keys)
- Async/Await (Tüm DB işlemleri)
- Entity ↔ DTO Mapping

## 📊 Veri Modeli

**Entities:**
- Category (1) ↔ (N) Product
- Customer (1) ↔ (N) Order
- Order (1) ↔ (N) OrderItem
- Product (1) ↔ (N) OrderItem
- BaseEntity: CreatedDate, UpdatedDate, IsDeleted

**Veri Akışı (Create Category):**
```
POST Request → Controller → Service Validation 
→ Business Rule Check → Repository → Database 
→ Mapper: DTO'ya dönüştür → 201 Response
```

## 🔐 Exception Handling

Merkezi ExceptionMiddleware tüm hataları yakalar ve uygun HTTP statüsü döner:

- ValidationMyException → 400 Bad Request (validasyon kuralı ihlali)
- NotFoundException → 404 Not Found (kayıt bulunamadı)
- BusinessRuleException → 409 Conflict (iş kuralı ihlali)
- Exception → 500 Internal Server Error

## 🤝 Gelecek Geliştirmeler

- [ ] JWT Authentication
- [ ] API Versioning
- [ ] Caching (Redis)
- [ ] Comprehensive Unit Tests
- [ ] Integration Tests
- [ ] Swagger/OpenAPI Dokumentasyonu
- [ ] Logging

---

**Not:** Bu proje professional backend mimarisi ve SOLID prensiplerinin pratik uygulamasını göstermektedir.
