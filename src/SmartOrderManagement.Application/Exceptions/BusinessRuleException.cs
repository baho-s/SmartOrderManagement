using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Exceptions
{
    public class BusinessRuleException:Exception
    {
        //Exception sınıfından özellik ve metotları devralarak kodun yeniden kullanılmasını sağlarız. 
        public BusinessRuleException(string message):base(message)
        {
            //BusinessRuleException, Exception'un constructor'ını :base() ile çağırır ve message gönderir.        
            //Exceptionda oluşan yapı bu durumda şu olur.
            /* public class Exception{
                 public Exception(string message){
                    //message ile işlemler gerçekleşir.            
                }                  
              }         
             */
            
            // Bu constructor dışarıdan gelen hata mesajını
            // base Exception sınıfına gönderir
            //
            // Yani:
            // throw new BusinessRuleException("Bu kategori adı zaten mevcut.");
            //
            // dediğimizde bu mesaj Exception.Message içine yerleşir
        }
    }
}
