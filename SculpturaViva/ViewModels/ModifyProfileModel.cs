using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.ViewModels
{
    public class ModifyProfileModel
    {
       
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        
    }
}
