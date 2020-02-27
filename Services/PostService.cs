using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCore.Data;
using BlogCore.Models;
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

        public async Task<bool> CreatePostAsync(CreatePostViewModel newPost)
        {
            var post = new Post()
            {
                Title = newPost.Title,
                BodyText = newPost.BodyText
            };

            _blogDbContext.Posts.Add(post);
            var saveResult = await _blogDbContext.SaveChangesAsync();

            return saveResult == 1;

        }

        public async Task<bool> EditPostAsync(EditPostViewModel post)
        {
            
            var postToUpdate = new Post()
            {
                Id = post.Id,
                Title = post.Title,
                BodyText = post.BodyText
            };
            _blogDbContext.Posts.Update(postToUpdate);

            var saveResult = await _blogDbContext.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _blogDbContext.Posts.ToListAsync();
        }

        //id should not be nullable so no need for int? id
        public async Task<Post> GetPost(int id)
        {
            var postDetail = await _blogDbContext.Posts.FirstAsync(post => post.Id == id);
            return postDetail;
        }

        public async Task<bool> RemovePostAsync(int id)
        {
            var postDetail = await GetPost(id);
            _blogDbContext.Posts.Remove(postDetail);
            var saveResult = await _blogDbContext.SaveChangesAsync();

            return saveResult == 1;
        }

    }
}