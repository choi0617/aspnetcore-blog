using System;

namespace BlogCore.Models
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}