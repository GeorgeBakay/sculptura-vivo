using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DataOfLike { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public UserDetailInfo User { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public Post Post { get; set; }

    }
}
