using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Interface
{
    public interface ILike
    {
        public Task<bool> LikeDisslike(string userName, int idPost);
    }
}
