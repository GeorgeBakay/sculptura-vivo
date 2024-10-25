using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;

namespace UBox.ViewModels
{
    public class SearchModel
    {
        [Required]
        public string Name { get; set; }
        public Dictionary<User,string> ListOfUser { get; set; }
    }
}
