using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;
using UBox.ViewModels;

namespace UBox.Date.Interface
{
    public interface IPost
    {
        public Dictionary<Post, string> getRecomendetPost(string userName);
        public List<Post> getPosts(string userName);
        public Task addPost(string userName,AddPostModel model,string filePath);
    }
}
