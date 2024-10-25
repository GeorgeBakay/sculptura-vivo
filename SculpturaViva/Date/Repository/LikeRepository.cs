using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;

namespace UBox.Date.Repository
{
    public class LikeRepository : ILike
    {
        private readonly AppDBContext appDBContext;
        public LikeRepository(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task<bool> LikeDisslike(string userName, int idPost)
        {
            UserDetailInfo thisUser = await appDBContext.UserDetailInfos.Include(u => u.likes).FirstOrDefaultAsync(o => o.user.UserName == userName);
            Post thisPost = await appDBContext.Posts.FirstOrDefaultAsync(u => u.Id == idPost);
            List<Like> listLike = thisUser.likes.ToList();
            Like thisLike = listLike.FirstOrDefault(u => u.PostId == idPost);
            if(thisLike != null)
            {
                appDBContext.Likes.Remove(thisLike);
                await appDBContext.SaveChangesAsync();
                return false;
            }
            else
            {
                Like newLike = new Like() {
                    DataOfLike = DateTime.Now,
                    Post = thisPost,
                    PostId = thisPost.Id,
                    User = thisUser,
                    UserId = thisUser.Id
                };
                appDBContext.Likes.Add(newLike);
                await appDBContext.SaveChangesAsync();
                return true;
            }

        }
    }
}
