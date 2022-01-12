using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Models;
using System.Configuration;

namespace WebApi.DataBaseProvider
{
    public abstract class DataBaseProviderBase
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        protected readonly static string ConnectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=HelpCodeDB;Integrated Security=True";

        public abstract Role GetDefaultRole();
        public abstract Role GetRoleByName(string name);

        public abstract RefreshToken GetRefreshToken(Guid id);
        public abstract int UpdateRefreshToken(RefreshToken refreshToken);

        public abstract Post GetPostById(Guid id);
        public abstract IEnumerable<Post> GetPosts(int page, int perPage = 100);
        public abstract IEnumerable<Post> GetPostsByCatgeryId(Guid id, int page, int perPage = 100);
        public abstract IEnumerable<Post> GetPostsByUserId(Guid id,int page, int perPage = 100);
        public abstract IEnumerable<Post> GetInactivePosts(int page, int perPage = 100);
        public abstract int AddPost(Post post);
        public abstract int UpdatePost(Post post);
        public abstract int DeactivatePostById(Guid id);

        public abstract User GetUserById(Guid id);
        public abstract IEnumerable<User> GetUsers(int page, int perPage = 100);
        public abstract int AddUser(User user);
        public abstract int UpdateUser(User user);
        public abstract int DeleteUserById(Guid id);
        public abstract User GetUserByEmail(string email);
        public abstract string GetRoleNameByUserId(Guid id);

        public abstract IEnumerable<Category> GetCategories(int page, int perPage = 100);
        public abstract int AddPostCategory(Guid postId, Guid categoryId);
        public abstract int RemovePostCategory(Guid postId, Guid categoryId);
        public abstract int AddCategory(string name, string description);
        public abstract Category GetCategoryByName(string name);
        public abstract Category GetCategoryById(Guid id);
        public abstract IEnumerable<Category> GetCategoriesByPostId(Guid id);

        public abstract int AddComment(Comment comment);
        public abstract IEnumerable<Comment> GetCommentsByPostId(Guid id);

        public abstract IEnumerable<object> Query1(string username, DateTime startDate, DateTime endDate);
        public abstract IEnumerable<object> Query2(string username, DateTime startDate, DateTime endDate);
        public abstract IEnumerable<object> Query3(string postTitle);
    }
}
