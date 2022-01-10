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
        protected readonly static string ConnectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=HelpCodeDB;Integrated Security=True";/*ConfigurationManager
            .ConnectionStrings["HelpCodeDb"]
            .ConnectionString;*/



        public abstract Role GetDefaultRole();
        public abstract Role GetRoleByName(string name);



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
    }
}
