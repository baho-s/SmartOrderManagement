using SmartOrderManagement.Domain.Entities;
namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(AppUser appUser);
        // AppUser alır → JWT Token döner
        // İçine CustomerId, Email claim olarak gömülecek
    }
}
