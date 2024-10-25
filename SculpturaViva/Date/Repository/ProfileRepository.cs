using Microsoft.AspNetCore.Http;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;
using Microsoft.EntityFrameworkCore;

namespace UBox.Date.Repository
{
    public class ProfileRepository:IProfile
    {
        public AppDBContext appDBContext;
        public ProfileRepository(AppDBContext _appDBContext)
        {
            this.appDBContext = _appDBContext;              
        }

        public async Task<List<User>> GetfollowerBy(User user)
        {
            UserDetailInfo thisUserwithReference = await appDBContext.UserDetailInfos.Include(u => u.follower).ThenInclude(t => t.FollowerUser).ThenInclude(k => k.user).FirstOrDefaultAsync(o => o.user == user);
            List<FollowArray> followArrays = thisUserwithReference.follower.ToList();
            List<User> result = new List<User>();
            foreach(var el in followArrays)
            {
                result.Add(el.FollowerUser.user);
            }
            return result;
        }

        public async Task<List<User>> GetfollowingBy(User user)
        {
            UserDetailInfo thisUserwithReference = await appDBContext.UserDetailInfos.Include(u => u.following).ThenInclude(t => t.FollowingUser).ThenInclude(k => k.user).FirstOrDefaultAsync(o => o.user == user);
            List<FollowArray> followArrays = thisUserwithReference.following.ToList();
            List<User> result = new List<User>();
            foreach (var el in followArrays)
            {
                result.Add(el.FollowingUser.user);
            }
            return result;
        }

        public int GetIdByName(string userName)
        {
            return appDBContext.Users.FirstOrDefault(u => u.UserName == userName).Id;
        }

        public User GetProfileById(int id)
        {
            return appDBContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User MyProfile(string UserName) {
             return appDBContext.Users.FirstOrDefault(u => u.UserName == UserName);
        }

        public List<User> SearchProfile(string UserName)
        { 
            IEnumerable<User> userEnum = appDBContext.Users.Where(u => u.UserName.StartsWith(UserName));
            return userEnum.ToList();
        }
    }
}
