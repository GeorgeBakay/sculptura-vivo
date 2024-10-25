using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;

namespace UBox.Date.Repository
{
    public class FollowArrayRepository : IFollowArray
    {
        AppDBContext appDBContext;
        public FollowArrayRepository(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }

        public bool checkOnFollow(string FollowerName, string FollowingName)
        {
            UserDetailInfo FollowerUser = appDBContext.UserDetailInfos.FirstOrDefault(u => u.user.UserName == FollowerName);
            UserDetailInfo FollowingUser = appDBContext.UserDetailInfos.FirstOrDefault(u => u.user.UserName == FollowingName);
            if (FollowerName == null && FollowingUser == null)
            {
                return false;
            }
            else
            {
                FollowArray array = appDBContext.FollowArrays.FirstOrDefault(u => u.FollowerUser == FollowerUser && u.FollowingUser == FollowingUser);
                if(array == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }

        public async Task<bool> FollowUnFollow(string FollowerName, string FollowingName)
        {
            UserDetailInfo FollowerUser = appDBContext.UserDetailInfos.FirstOrDefault(u => u.user.UserName == FollowerName);
            UserDetailInfo FollowingUser = appDBContext.UserDetailInfos.FirstOrDefault(u => u.user.UserName == FollowingName);
            if(FollowerName == null && FollowingUser == null)
            {
                return false;
            }
            else
            {
                FollowArray array = appDBContext.FollowArrays.FirstOrDefault(u => u.FollowerUser == FollowerUser && u.FollowingUser == FollowingUser);
                if(array != null)
                {
                    appDBContext.FollowArrays.Remove(array);
                    await appDBContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    appDBContext.FollowArrays.Add(new FollowArray
                    {
                        FollowerUser = FollowerUser,
                        FollowerUserId = FollowerUser.Id,
                        FollowingUser = FollowingUser,
                        FollowingUserId = FollowingUser.Id
                    });
                    await appDBContext.SaveChangesAsync();
                    return false;
                } 
            }
        }
    }
}
