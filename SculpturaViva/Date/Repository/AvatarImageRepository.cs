using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Interface;
using UBox.Date.Models;

namespace UBox.Date.Repository
{
    public class AvatarImageRepository : IAvatarImage
    {
        AppDBContext appDBContext;
        public AvatarImageRepository(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public UserAvatarImage getAvatarImage(User User)
        {

            UserDetailInfo result = appDBContext.UserDetailInfos.Include(o => o.avatar).FirstOrDefault(u => u.user == User);
            return result.avatar;
        }
    }
}
