using FluentValidation;
using Microsoft.IdentityModel.Tokens.Experimental;
using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.Exceptions;
using System;
using System.Text.Json;

namespace SmartOrderManagement.API.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        //Pipeline'daki bir sonraki middleware'i temsil eder
        //
        //RequestDelegate nedir?
        //Gelen isteği bir sonraki adıma gönderen temsilcidir
        //
        //Yani bu middleware işi bittikten sonra
        //Request'i sıradaki middleware'e veya controller'a iletir

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //Constructor ile gelen next parametresini field'a atıyoruz
            //
            //Böylece bu middleware'den sonra çalışacak olan yapıyı tutmuş oluyoruz.
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // InvokeAsync middleware'in çalıştığı ana metottur
            //
            // ASP.NET Core request geldiğinde bu metodu çalıştırır
            //
            // HttpContext nedir?
            // Gelen istek ve dönecek cevap ile ilgili tüm bilgileri taşır

            try
            {
                await _next(context);
                // Request'i pipeline'daki bir sonraki adıma gönderiyoruz
                //
                // Eğer aşağı tarafta (controller, service, repository vs.)
                // hiç hata oluşmazsa akış normal devam eder
            }
            catch (Exception ex)
            {
                //LOGLAMA DAHA SONRA ÖNEMLİ BAKILACAK.
                //_logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message); LOG'LAMAYI DENEDİM OLMADI.
                //Hatayı logluyoruz.
                //Production'da bu loglar dosyaya/Seq/ElasticSearch yazılır.

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            // Dönen cevabın tipini JSON olarak ayarlıyoruz
            //
            // Çünkü kullanıcıya düz yazı değil
            // JSON formatında hata bilgisi dönmek istiyoruz

            //*** context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            // Şimdilik tüm hataları 500 Internal Server Error olarak döndürüyoruz
            //
            // Bu başlangıç için normal
            // İleride exception tipine göre 400, 404 gibi ayıracağız ***

            context.Response.StatusCode = ex switch
            {
                // 1️ VALIDATION HATASI (422)
                ValidationMyException => StatusCodes.Status422UnprocessableEntity,

                // 2 İŞ KURALI HATASI
                BusinessRuleException => StatusCodes.Status400BadRequest,

                // 3 BULUNAMADI HATASI
                NotFoundException => StatusCodes.Status404NotFound,

                // 4 Diğer Tüm Hatalar(500)
                //Beklenmeyen tüm hatalar: NullReference, DB bağlantı
                _ => StatusCodes.Status500InternalServerError
            };

            var response = ex switch
            {
                //Validation Hatası
                ValidationMyException validatonEx => new
                ApiResponse<object>
                {
                    IsSucces = false,
                    StatusCode = context.Response.StatusCode,
                    Message = "Doğrulama hataları oluştu.",//Genel bir mesaj veriyoruz.
                    Data = null,
                    Errors = validatonEx.ErrorDetails
                },
                //İş Kuralı Hatası
                BusinessRuleException businessEx=> new
                ApiResponse<object>
                {
                    IsSucces=false,
                    StatusCode=context.Response.StatusCode,
                    Message=businessEx.Message,//Direkt iş kuralı mesajını dön Kategori zaten mevcut.
                    Data=null,
                    Errors=new List<string> { businessEx.Message}
                },
                //Bulunamadı Hatası
                NotFoundException notFoundEx=>new
                ApiResponse<object>
                {
                    IsSucces=false,
                    StatusCode=context.Response.StatusCode,
                    Message=notFoundEx.Message,//"Category {Anahtar: 5} bulunamadı."
                    Data=null,
                    Errors=new List<string> { notFoundEx.Message}
                },
                //Diğer bütün hatalarda 500
                _ =>new ApiResponse<object>
                {
                    IsSucces=false,
                    StatusCode=context.Response.StatusCode,
                    Message=ex.Message,
                    //Message="Sunucuda beklenmeyen hata oluştu.", //BURAYI AÇICAZ TEST İÇİN KAPATIP EX MESSAGE EKLEDİK.
                    // ↑ Kullanıcıya genel mesaj ver
                    // ASLA ex.Message verme! (güvenlik açığı)
                    // ex.Message: "SQL connection failed at 192.168.1.1:1433"
                    // Bu bilgiyi hacker'a vermek istemezsin
                    Data = null,
                    Errors=new List<string> {"Lütfen daha sonra tekrar deneyin." }
                }

            };


            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                // C# PascalCase → JSON camelCase
                // IsSucces → isSucces
                // StatusCode → statusCode
                // Frontend genelde camelCase bekler
            };


            var jsonResponse = JsonSerializer.Serialize(response,jsonOptions);
            // C# nesnesini JSON string'e çeviriyoruz
            //
            // Çünkü HTTP response içine doğrudan object değil
            // string/json yazılır

            return context.Response.WriteAsync(jsonResponse);
            // Oluşturduğumuz JSON hata cevabını kullanıcıya yazıyoruz
            //
            // Böylece hata olduğunda istemci düzgün bir JSON response alır

        }
    }
}
