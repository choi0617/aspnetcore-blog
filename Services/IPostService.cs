using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCore.Models;

namespace BlogCore.Services
{
    public interface IPostService
    {
         Task<IEnumerable<Post>> GetAllPostsAsync();

         Task<bool> CreatePostAsync(CreatePostViewModel newPost);

         Task<bool> RemovePostAsync(int id);

         //Task<bool> EditPostAsync(Post post); (original)

         Task<bool> EditPostAsync(EditPostViewModel post);

        //id should not be nullable so no need for int? id
         Task<Post> GetPost(int id);
         

    }
}