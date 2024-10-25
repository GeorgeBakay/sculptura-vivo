using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using UBox.Date.Interface;
using UBox.ViewModels;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Authentication;


namespace UBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProfile _profile;
        private readonly IAvatarImage _avatar;
        private readonly IPost _post;
        private readonly ILike _like;
        public IndexModel obj;
        public HomeController(IProfile profile, IAvatarImage avatar,IPost post,ILike like)
        {
            _profile = profile;
            _avatar = avatar;
            _post = post;
            _like = like;
            obj = new IndexModel();
        }




        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
            
            obj.user = _profile.MyProfile(User.Identity.Name);
            if(obj.user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "User");
            }
            string imreBase64Data = Convert.ToBase64String(_avatar.getAvatarImage(obj.user).ImageData);
            obj.imageDataUrl = string.Format("data:image/png;base64,{0}", imreBase64Data);
            obj.ListOfPosts = _post.getRecomendetPost(User.Identity.Name);
            return View(obj);
        }
  
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<bool> Like(string postIdStr)
        {
            int postId = Convert.ToInt32(postIdStr);
            bool result = await _like.LikeDisslike(User.Identity.Name,postId);
            return result;
        }
    }
}
