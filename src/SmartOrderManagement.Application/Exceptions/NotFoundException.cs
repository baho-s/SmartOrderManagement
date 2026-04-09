using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Exceptions
{
    // NE ZAMAN KULLANILIR?
    // ID ile aranan kayıt veritabanında yoksa
    public class NotFoundException : Exception
    {
        /// Aranan kayıt bulunamadığında fırlatılır.
        public NotFoundException(string entityName, object key)
            : base($"{ entityName}  (Anahtar: {key}) bulunamadı.")
        {
            // Kullanım: throw new NotFoundException("Kategori", 5);
            // Mesaj: "Kategori (ID: 5) bulunamadı."
        }

        public NotFoundException(string message):base(message) 
        {
            
        }
    }
}
