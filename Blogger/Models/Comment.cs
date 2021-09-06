using System.ComponentModel.DataAnnotations;

namespace Blogger.Models
{
    public class Comment
    {
       public int Id { get; set; }
       [Required]
       [MaxLength(240)]
       public string Body { get; set; }
       [Required]
       public int BlogId { get; set; } 

       public string CreatorId { get; set; }
       //having profile isn't in the instructions, but it is on the blog so that it can populate, so should it be here?
       public Profile Creator { get; set; }
    }
}