using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogCore.Services
{
    public interface IPostService
    {
         Task<IEnumerable<Post>> GetAllPostsAsync();
         Task<IEnumerable<Post>> GetAllUserPostsAsync(IdentityUser user);

         Task<bool> CreatePostAsync(CreatePostViewModel newPost, IdentityUser user);

         Task<bool> RemovePostAsync(int id, string currentUserId);

         Task<bool> EditPostAsync(EditPostViewModel post, IdentityUser user);

        //id should not be nullable so no need for int? id
         Task<Post> GetPost(int id);
         Task<Post> GetUserPost(int id, string currentUserId);
         

    }
}