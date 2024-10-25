using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Models
{
    public class FollowArray
    {
        [Key]
        public int Id { get; set; }
        //який користувач підписаний
        public int FollowerUserId { get; set; }
        public UserDetailInfo FollowerUser { get; set; }


        //На кого користувач підписаний
        public int FollowingUserId { get; set; }
        public UserDetailInfo FollowingUser { get; set; }
       
        
    }
}
