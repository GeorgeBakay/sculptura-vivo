using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Interface
{
    public interface IFollowArray
    {
        public Task<bool> FollowUnFollow(string FollowerName, string FollowingName);
        public bool checkOnFollow(string FollowerName, string FollowingName);
    }
}
