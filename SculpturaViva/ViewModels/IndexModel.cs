using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UBox.ViewModels
{
    public class IndexModel
    {
        public User user { get; set; }
        public string imageDataUrl { get; set; }

        public Dictionary<Post,string> ListOfPosts { get; set; }
    }
}
