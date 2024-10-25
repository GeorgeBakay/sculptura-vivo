using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Models
{
    public class UserDetailInfo
    {
        [Key]
        public int Id { get; set; }
        //Користувач (власник данних)
        public User user { get; set; }
        public int? userId { get; set; }
        public int avatarId { get; set; }
        public UserAvatarImage avatar { get; set; } = null;
        //Список постів користувача
        public virtual ICollection<Post> posts { get; set; } = new List<Post>();
        //Список користувачів на яких підписано
        public virtual ICollection<FollowArray> follower { get; set; } = new List<FollowArray>();
        //список користувачів які підписалися на вас
        public virtual ICollection<FollowArray> following { get; set; } = new List<FollowArray>();
        //список вподобань користувача
        public virtual ICollection<Like> likes { get; set; } = new List<Like>();

    }
}
