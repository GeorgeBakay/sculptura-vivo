using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.ViewModels
{
    public class LoginModel
    {
        
        [Required(ErrorMessage = "Ім'я користувача не вказано")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пароль не вказаний")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
