using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;
using UBox.ViewModels;

namespace UBox.Controllers
{
    public class SearchController : Controller
    {
        private readonly IProfile _profile;
        private readonly IAvatarImage _avatarImage;
        public SearchController(IProfile profile,IAvatarImage avatarImage)
        {
            _avatarImage = avatarImage;
            _profile = profile;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult ProfileSearch()
        {
            return View();
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult ProfileSearch(SearchModel model)
        {
            SearchModel obj = new SearchModel();
            obj.Name = model.Name;
            obj.ListOfUser = new Dictionary<User, string>();
            List<User> users = _profile.SearchProfile(model.Name);
            foreach(var el in users)
            {
                byte[] image = _avatarImage.getAvatarImage(el).ImageData;
                string imreBase64Data = Convert.ToBase64String(image);
                obj.ListOfUser.Add(el,  string.Format("data:image/png;base64,{0}", imreBase64Data));
            }
            
            return View(obj);
        }
    }
}
