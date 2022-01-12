using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.Interfaces;
using Newtonsoft.Json;
using WebApi.DataBaseProvider;
using WebApi.Entities;
using WebApi.Infrastructure.Extensions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[post]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;

        private readonly DataBaseProviderBase _dbProvider;

        public PostController(ILogger<PostController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dbProvider = dataBaseService.GetProvider();
        }

        [HttpGet("/getposts")]
        public IEnumerable<Post> GetPosts(int page, int perPage = 100)
        {
            var posts = _dbProvider.GetPosts(page, perPage);

            return posts;
        }

        [HttpGet("/getpostbyid")]
        public Post GetPostById(Guid id)
        {
            return _dbProvider.GetPostById(id);
        }

        [Authorize]
        [HttpPost("/addpost")]
        public int AddPost(Post post)
        {
            if (_dbProvider.GetPostById(post.Id) != null)
            {
                return _dbProvider.UpdatePost(post);
            }

            post.OwnerUserId = Guid.Parse(User.Claims.GetValueByType("Id"));

            return _dbProvider.AddPost(post);
        }

        [HttpGet("/getpostsbyuserid")]
        public IEnumerable<Post> GetPostsByUserId(Guid id, int page, int perPage = 100)
        {
            var posts = _dbProvider.GetPostsByUserId(id, page, perPage);

            return posts;
        }

        [HttpGet("/getpostsbycategoryid")]
        public IEnumerable<Post> GetPostsByCategoryId(Guid id, int page, int perPage = 100)
        {
            var posts = _dbProvider.GetPostsByCatgeryId(id, page, perPage);

            return posts;
        }

        [Authorize]
        [HttpPost("/addcomment")]
        public int AddComment(Comment comment)
        {
            comment.OwnerUserId = Guid.Parse(User.Claims.GetValueByType("Id"));

            return _dbProvider.AddComment(comment);
        }

        [HttpGet("/getcommentsbypostid")]
        public IEnumerable<Comment> GetPostsByCategoryId(Guid id)
        {
            return _dbProvider.GetCommentsByPostId(id);
        }
    }
}
