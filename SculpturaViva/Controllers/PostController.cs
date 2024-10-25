using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.ViewModels;

namespace UBox.Controllers
{
    public class PostController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IPost _post;
        public PostController(IHostEnvironment hostEnvironment,IPost post)
        {
            _post = post;
            _hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddPost(AddPostModel model)
        {
            if(model.PostItem != null)
            {
                string path = "/DataOfClients/Posts/" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "_" + model.PostItem.FileName;
                using (var fileStream = new FileStream(_hostEnvironment.ContentRootPath + "/wwwroot" +  path, FileMode.Create))
                {
                    await model.PostItem.CopyToAsync(fileStream);
                }
                await _post.addPost(User.Identity.Name, model, path);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Виберіть фото");
            return View();
        }
    }
}
