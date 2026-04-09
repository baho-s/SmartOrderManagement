using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Common.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSucces { get; set; }
        //İşlem başarılı mı? Frontend ilk buna bakar.
        public int StatusCode { get; set; }
        //HTTP status code bilgisini taşır(200,400,500 vb.).
        public string Message { get; set; }
        //Kullanıcıya gösterilecek açıklama mesajı.
        public T Data { get; set; }
        //Asıl veri burada tutulur.
        //Generic olduğu için her tip olabilir:
        //CategoryDto, List<CategoryDto>, string vs.
        public List<string> Errors { get; set; }
        //Hata detaylarını tutar.
        //Validation hatalarında kullanılır.

        public static ApiResponse<T> Succes(T data,string message="İşlem başarılı")
        {
            return new ApiResponse<T>
            {
                IsSucces = true,
                StatusCode = 200,
                Message = message,
                Data = data,
                Errors=null //Başarılıysa hata yok.
            };
        }

        public static ApiResponse<T> Fail(string message,int statusCode,List<string>? errors=null)
        {
            return new ApiResponse<T>
            {
                IsSucces = false,
                StatusCode = statusCode,
                Message = message,
                Data=default, //Hatalıysa data yok(null)
                Errors = errors ?? new List<string>()
                // errors null gelirse boş liste ata
                // Böylece frontend null check yapmak zorunda kalmaz
            };
        }
        //Succes ve Fail methodlarının amacı nedir?
        //Hata response'u üretir.
        //Frontend her zaman aynı yapıyı alır.

       
    }
}
