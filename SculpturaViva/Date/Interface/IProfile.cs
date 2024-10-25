using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;

namespace UBox.Date.Interface
{
    public interface IProfile
    {
        public User MyProfile(string UserName);
        public List<User> SearchProfile(string UserName);
        public User GetProfileById(int id);
        public int GetIdByName(string userName);
        public Task<List<User>> GetfollowerBy(User user);
        public Task<List<User>> GetfollowingBy(User user);



    }
}
