using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _blogDbContext;

        public PostService(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<bool> CreatePostAsync(CreatePostViewModel newPost, IdentityUser user)
        {
            var post = new Post()
            {
                Title = newPost.Title,
                BodyText = newPost.BodyText,
                UserId = user.Id
            };

            _blogDbContext.Posts.Add(post);
            var saveResult = await _blogDbContext.SaveChangesAsync();

            return saveResult == 1;

        }

        public async Task<bool> EditPostAsync(EditPostViewModel post, IdentityUser user)
        {
            
            var postToUpdate = new Post()
            {
                Id = post.Id,
                Title = post.Title,
                BodyText = post.BodyText,
                UserId = user.Id,
            };
            _blogDbContext.Posts.Update(postToUpdate);

            var saveResult = await _blogDbContext.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _blogDbContext.Posts
                            .Include(x => x.User)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllUserPostsAsync(IdentityUser user)
        {
            return await _blogDbContext.Posts.Where(x => x.UserId == user.Id).ToListAsync();
        }


        //id should not be nullable so no need for int? id
        public async Task<Post> GetPost(int id)
        {
            var postDetail = await _blogDbContext.Posts
                    .Include(x => x.User)
                    .FirstAsync(post => post.Id == id);
            return postDetail;
        }

        public async Task<Post> GetUserPost(int id, string currentUserId)
        {
            return  await _blogDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId);
        }

        public async Task<bool> RemovePostAsync(int id, string currentUserId)
        {
            var postDetail = await GetUserPost(id, currentUserId);

            if(postDetail != null)
            {
                _blogDbContext.Posts.Remove(postDetail);
                var saveResult = await _blogDbContext.SaveChangesAsync();
                return saveResult == 1;  
            }
            
            return false;                     
        }

    }
}