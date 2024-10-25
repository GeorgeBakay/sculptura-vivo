using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;
using UBox.ViewModels;

namespace UBox.Date.Repository
{
    public class PostRepository : IPost
    {
        private readonly AppDBContext appDBContext;
        public PostRepository(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }

        public async Task addPost( string userName, AddPostModel model,string filepath)
        {
            UserDetailInfo thisUser = appDBContext.UserDetailInfos.FirstOrDefault(u => u.user.UserName == userName);
            List<string> tegs = new List<string> { };
            string type = Path.GetExtension(model.PostItem.FileName).ToLower();
            if(model.Description != null)
            {
                if (model.Description.Contains('#'))
                {
                    foreach (string teg in model.Description.Split(' '))
                    {
                        if (teg.Contains('#'))
                        {
                            string[] tegMass = teg.Split("#");
                            foreach (string el in tegMass)
                            {
                                tegs.Add(el);
                            }
                        }
                    }
                }
            }
            Post thisPost = new Post {
                PostFilePath = filepath,
                FileType = type,
                Description = model.Description,
                PublishDate = DateTime.Now,
                User = thisUser,
            };
            appDBContext.Posts.Add(thisPost);
            //thisUser.posts.Add(thisPost);
            await appDBContext.SaveChangesAsync();
            
           
        }

        public List<Post> getPosts(string userName)
        {

            var user = appDBContext.UserDetailInfos.Include(p => p.posts).FirstOrDefault(u => u.user.UserName == userName);

            List <Post> posts = user.posts.ToList();
            if(posts != null)
            {
                return posts.OrderByDescending(u => u.PublishDate).ToList();
            }
            return new List<Post>();
            
        }

        public Dictionary<Post,string> getRecomendetPost(string userName)
        {
            var user = appDBContext.UserDetailInfos
                .Include(p => p.follower).ThenInclude(i => i.FollowingUser).ThenInclude(o => o.posts)
                .Include(p => p.follower).ThenInclude(i => i.FollowingUser).ThenInclude(o => o.avatar)
                .Include(p => p.follower).ThenInclude(i => i.FollowingUser).ThenInclude(o => o.user)
                .FirstOrDefault(u => u.user.UserName == userName);
            List<FollowArray> usersFollowingArray = user.follower.ToList();
            Dictionary<Post, string> recomendetPost = new Dictionary<Post, string>();
            if(usersFollowingArray != null)
            {
                foreach (FollowArray el in usersFollowingArray)
                {
                    List<Post> posts = el.FollowingUser.posts.ToList();
                    foreach(Post elpost in posts)
                    {
                        recomendetPost.Add(elpost, string.Format("data:image/png;base64,{0}", Convert.ToBase64String(el.FollowingUser.avatar.ImageData)));
                    }
                    
                }
            }
            recomendetPost = recomendetPost.OrderByDescending(u => u.Key.PublishDate).ToDictionary(u =>u.Key,u => u.Value);
            return recomendetPost;
        }
    }
}
