using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UBox.Date;
using UBox.Date.Models;
using UBox.ViewModels;

namespace UBox.Controllers
{
    public class UserController : Controller
    {
        private AppDBContext db;
        private readonly IHostEnvironment _hostingEnvironment;

        public UserController(AppDBContext context, IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            db = context;
        } 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string hash = getHashCode(model.Password);
                User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == hash);
                if (user != null) 
                {
                    await Authenticate(model.UserName); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некоректний логін або пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName || u.Email == model.Email);

                if (model.Password.Length < 8)
                {
                    ModelState.AddModelError("", "Пароль повинен містити мінімум 8 символів");
                }
                else if (!model.Email.Contains("@"))
                {
                    ModelState.AddModelError("", "Формат email введений невірно");
                }
                else if (user == null)
                {
                    string hash = getHashCode(model.Password);
                    byte[] bytes;
                    if (model.Image != null)
                    {
                        var file = model.Image;
                        var imageFileStream = file.OpenReadStream();
                        Bitmap image = new Bitmap(imageFileStream);//image value is bitmap type
                        float compress = 300 / (float)image.Height;
                        Bitmap resizedImage = new Bitmap(image, new Size((int)(image.Width * compress), 300));
                        var ms = new MemoryStream();
                        resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bytes = ms.GetBuffer();
                    }
                    else
                    {
                        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/img", "avatar.jpg");
                        var imageFileStream = System.IO.File.OpenRead(path);
                        bytes = new byte[imageFileStream.Length];
                        imageFileStream.Read(bytes, 0, (int)imageFileStream.Length);

                    }
                    User userAdder = new User { UserName = model.UserName,
                        Email = model.Email,
                        Password = hash,
                        DateCreate = DateTime.Now 
                    };
                    UserDetailInfo userDetailInfoAdder = new UserDetailInfo
                    {
                        user = userAdder,
                        posts = new List<Post>(),
                        follower = new List<FollowArray>(),
                        following = new List<FollowArray>()
                    };
                    userAdder.userDetailInfo = userDetailInfoAdder;
                    db.Users.Add(userAdder);
                    db.UserDetailInfos.Add(userDetailInfoAdder);

                    await db.SaveChangesAsync();


                    UserDetailInfo addUser = await db.UserDetailInfos.FirstOrDefaultAsync(u => u.user.UserName == model.UserName);
                    db.AvatarImages.Add(new UserAvatarImage {ImageData = bytes,UserId = addUser.Id,User = addUser});


                    await db.SaveChangesAsync();
                    await Authenticate(model.UserName);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Користувач з таким іменем або email вже існує");
            }
            return View(model);
        }
        private string getHashCode(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in byteHash)
            {
                sbHash.Append(string.Format("{0:x2}", b));
            }
            string hash = sbHash.ToString();
            return hash;
        }
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}
