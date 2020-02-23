using System;
using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime Created {get; set;} = DateTime.Now;
    }
}