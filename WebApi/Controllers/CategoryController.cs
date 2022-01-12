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
    [Route("[category]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;

        private readonly DataBaseProviderBase _dbProvider;

        public CategoryController(ILogger<PostController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dbProvider = dataBaseService.GetProvider();
        }

        [HttpGet("/getcategories")]
        public IEnumerable<Category> GetCategories(int page, int perPage = 100)
        {
            var categories = _dbProvider.GetCategories(page, perPage);

            return categories;
        }
        [HttpGet("/addcategory")]
        public int AddCategory(string name, string description)
        {
            return _dbProvider.AddCategory(name, description);
        }
        [HttpGet("/addpostcategory")]
        public int AddPostCategory(Guid postId, string name)
        {
            var category = _dbProvider.GetCategoryByName(name);

            if (category == null)
            {
                _dbProvider.AddCategory(name, string.Empty);
                category = _dbProvider.GetCategoryByName(name);
            }

            return _dbProvider.AddPostCategory(postId, category.Id);
        }
        [HttpGet("/removepostcategory")]
        public int RemovePostCategory(Guid postId, Guid categoryId)
        {
            return _dbProvider.RemovePostCategory(postId, categoryId);
        }

        [HttpGet("/getcategoriesbypostid")]
        public IEnumerable<Category> GetCategoriesByPostId(Guid id)
        {
            return _dbProvider.GetCategoriesByPostId(id);
        }
    }
}
