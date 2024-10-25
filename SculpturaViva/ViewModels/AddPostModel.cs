using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;

namespace UBox.ViewModels
{
    public class AddPostModel
    {
        //Передається у View
        public List<User> Follow { get; set; }


        //Отримується з View
        [Required(ErrorMessage = "Додайте фото або відео")]
        [DataType(DataType.Upload)]
        public IFormFile PostItem { get; set; }
        public string Description { get; set; }
        public List<string> Friends { get; set; }
    }
}
