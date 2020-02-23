using System.Collections.Generic;

namespace BlogCore.Models
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}