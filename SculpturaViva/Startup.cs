using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UBox.Date;
using UBox.Date.Interface;
using UBox.Date.Repository;

namespace UBox
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("dbsettings.json").Build();
        }
       
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));
            services.AddMvc();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.ConsentCookie.IsEssential = true;
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.Cookie.IsEssential = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.Name = "Cookie";
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/User/Login");
               });


            services.AddScoped<IProfile,ProfileRepository>();
            services.AddScoped<IAvatarImage, AvatarImageRepository>();
            services.AddTransient<IPost, PostRepository>();
            services.AddTransient<IFollowArray, FollowArrayRepository>();
            services.AddTransient<ILike, LikeRepository>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          
            //app.UseHttpsRedirection();
            if (env.IsDevelopment())
            {
                //app.UseHsts();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            

            app.UseRouting();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
