using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogCore.Models;
using BlogCore.Services;

namespace BlogCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;

        public HomeController(ILogger<HomeController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post()
        {
            var posts = await _postService.GetAllPostsAsync();
            var model = new PostViewModel()
            {
                Posts = posts
            };

            return View(model);   
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) 
            {
                return BadRequest("400 Bad Request");
            }
            else
            {
                var post = await _postService.GetPost((int)id);
                
                var updatedPost = new EditPostViewModel()
                {
                    Id = post.Id,
                    Title = post.Title,
                    BodyText = post.BodyText,
                };

                return View(updatedPost);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel post)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Post");
            }

            var postToUpdate = await _postService.EditPostAsync(post);

            return RedirectToAction("Post");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel newPost)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _postService.CreatePostAsync(newPost);

            if(!successful)
            {
                return BadRequest("Could not create post");
            }

            return RedirectToAction("Post");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = await _postService.GetPost(id);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _postService.RemovePostAsync(id);
            return RedirectToAction("Post");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
