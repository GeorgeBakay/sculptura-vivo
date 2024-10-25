using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UBox.Date.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PostFilePath { get; set; }
        [Required]
        public string FileType { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public UserDetailInfo User { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        
    }
}
