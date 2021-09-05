using System.ComponentModel.DataAnnotations;

namespace Blogger.Models
{
public class Blog
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImgUrl { get; set; } = "https://via.placeholder.com/150";
        public bool Published { get; set; }
        public string CreatorId { get; set; }
        public Account Creator { get; set; }
    }
}