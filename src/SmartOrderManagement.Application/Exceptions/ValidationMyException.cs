using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Exceptions
{
    // NE ZAMAN KULLANILIR?
    // "Ad boş olamaz", "Fiyat negatif olamaz" gibi DOĞRULAMA hataları
    // Birden fazla hata olabilir, o yüzden liste tutuyoruz
    public class ValidationMyException:Exception
    {
        public List<string> ErrorDetails { get; }
        //Birden fazla validasyon hatası olabilir.


        public ValidationMyException(string message):base(message)
        {
            ErrorDetails = new List<string> { message};
        }

        public ValidationMyException(List<string> errors):base("Bir veya birden fazla doğrulama hatası oluştu.")
        {
            ErrorDetails = errors;
        }
    }
}
