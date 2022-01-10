using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    /// <summary>
    /// База данных.
    /// </summary>
    public class HelpCodeDb : DataConnection
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        protected readonly new string ConnectionString;

        /// <summary>
        /// Список пользователей.
        /// </summary>
        public ITable<User> Users => GetTable<User>();
        public ITable<Post> Posts => GetTable<Post>();
        public ITable<Category> Categories => GetTable<Category>();
        public ITable<Comment> Comments => GetTable<Comment>();
        public ITable<Role> Roles => GetTable<Role>();
        public ITable<PostsCategories> PostsCategories => GetTable<PostsCategories>();
        public ITable<RefreshToken> RefreshTokens => GetTable<RefreshToken>();

        /// <summary>
        /// Вызывает базовый конструктор.
        /// </summary>
        public HelpCodeDb(string connectionString) :
            base(ProviderName.SqlServer2017, connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
