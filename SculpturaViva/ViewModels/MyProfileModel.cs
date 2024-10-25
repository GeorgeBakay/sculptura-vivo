using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;

namespace UBox.ViewModels
{
    public class MyProfileModel
    {
        public User user { get; set; }
        public string imageAvatar { get; set; }
        public List<Post> posts { get; set; }
        public List<User> followers { get; set; }
        public List<User> following { get; set; }
    }
}
