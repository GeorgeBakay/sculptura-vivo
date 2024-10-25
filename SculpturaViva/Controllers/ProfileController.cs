using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.ViewModels;

namespace UBox.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfile _profile;
        private readonly IAvatarImage _avatar;
        private readonly IPost _post;
        private readonly IFollowArray _followArray;


        public ProfileController(IProfile profile, IAvatarImage avatar, IPost post , IFollowArray followArray)
        {
            _profile = profile;
            _avatar = avatar;
            _post = post;
            _followArray = followArray;
        }

        
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MyProfile()
        {
            MyProfileModel obj = new MyProfileModel();
            obj.user = _profile.MyProfile(User.Identity.Name);
            
            if (obj.user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "User");
            }
            byte[] image = _avatar.getAvatarImage(obj.user).ImageData;
            string imreBase64Data = Convert.ToBase64String(image);
            obj.imageAvatar = string.Format("data:image/png;base64,{0}", imreBase64Data);
            obj.followers = await _profile.GetfollowerBy(obj.user);
            obj.following = await _profile.GetfollowingBy(obj.user);

            obj.posts = _post.getPosts(User.Identity.Name);
            return View(obj);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Profile(int userId)
        {
            ProfileModel obj = new ProfileModel();
            obj.user = _profile.GetProfileById(userId);
            byte[] image;
            if (obj.user == null)
            {
                image = null;
            }
            else
            {
                image = _avatar.getAvatarImage(obj.user).ImageData;
            }
            string imreBase64Data = Convert.ToBase64String(image);
            obj.imageAvatar = string.Format("data:image/png;base64,{0}", imreBase64Data);
            obj.posts = _post.getPosts(obj.user.UserName);
            obj.isFollow = _followArray.checkOnFollow(User.Identity.Name, obj.user.UserName);
            obj.following = await _profile.GetfollowingBy(obj.user);
            obj.followers = await _profile.GetfollowerBy(obj.user);
            return View(obj);
        }

 
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<bool> Follow(string userName)
        {
            bool result = await _followArray.FollowUnFollow(User.Identity.Name, userName);
            return result/*RedirectToAction("Profile",new {userId = id})*/;
        }
    }
}
