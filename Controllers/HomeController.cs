using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogCore.Models;
using BlogCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IPostService postService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            var model = new PostViewModel()
            {
                Posts = posts
            };

            return View(model);  
        }

        public async Task<IActionResult> Post()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            var posts = await _postService.GetAllUserPostsAsync(currentUser);
            var model = new PostViewModel()
            {
                Posts = posts
            };

            return View(model);   
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) 
            {
                return BadRequest("400 Bad Request");
            }
            var currentUserId = _userManager.GetUserId(User);
            var post = await _postService.GetUserPost((int)id, currentUserId);
            
            if(post != null)
            {
                var updatedPost = new EditPostViewModel()
                {
                    Id = post.Id,
                    Title = post.Title,
                    BodyText = post.BodyText,
                };

                return View(updatedPost);
            }     
            
            return BadRequest("Not permitted to edit another person's post");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel post)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Post");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            var successful = await _postService.EditPostAsync(post, currentUser);

            if(!successful)
            {
                return BadRequest("Could not edit post");
            }

            return RedirectToAction("Detail", new {id = post.Id });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel newPost)
        {   
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _postService.CreatePostAsync(newPost, currentUser);

            if(!successful)
            {
                return BadRequest("Could not create post");
            }

            return RedirectToAction("Post");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Post");
            }
            var post = await _postService.GetPost((int)id);
            var model = new DetailPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                BodyText = post.BodyText,
                Created = post.Created,
                UserName = post.User.UserName,
            };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest($"Could not find post");
            }

            var currentUserId = _userManager.GetUserId(User);

            var successful = await _postService.RemovePostAsync((int)id, currentUserId);

            if(!successful)
            {
                return BadRequest("Could not delete");
            }
            return RedirectToAction("Post");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
