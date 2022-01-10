using System;
using System.Collections.Generic;
using WebApi.Entities;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using LinqToDB;

namespace WebApi.DataBaseProvider
{
    public class LinqToDbProvider : DataBaseProviderBase
    {

        #region #TokensProcessing

        public override RefreshToken GetRefreshToken(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.RefreshTokens
                    .Where(x => x.UserId == id)
                    .FirstOrDefault();
            }
        }

        public override int UpdateRefreshToken(RefreshToken refreshToken)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                database.RefreshTokens
                    .Where(x => x.UserId == refreshToken.UserId)
                    .Delete();

                return database.RefreshTokens
                    .Value(x => x.UserId, refreshToken.UserId)
                    .Value(x => x.ExpirationDate, refreshToken.ExpirationDate)
                    .Value(x => x.Token, refreshToken.Token)
                    .Insert();
            }
        }
        #endregion




        #region RolesProcessing
        public override Role GetDefaultRole()
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Roles
                    .Where(x => x.Name == "Default")
                    .FirstOrDefault();
            }
        }
        public override Role GetRoleByName(string name)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Roles
                    .Where(x => x.Name == name)
                    .FirstOrDefault();
            }
        }
        #endregion

        #region UserProcessing
        /// <summary>
        /// Возвращает пользователя по ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override User GetUserById(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Users
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Возвращает перечисление пользователей постранично
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override IEnumerable<User> GetUsers(int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Users
                    .Skip((page - 1) * perPage)
                    .Take(perPage)
                    .ToArray();
            }
        }

        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int AddUser(User user)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                user.Id = Guid.NewGuid();

                return database.Users
                    .Value(x => x.Id, user.Id)
                    .Value(x => x.Username, user.Username)
                    .Value(x => x.PasswordHash, user.PasswordHash)
                    .Value(x => x.Email, user.Email)
                    .Value(x => x.RoleId, GetDefaultRole().Id)
                    .Insert();
            }
        }

        /// <summary>
        /// Обновляет существующего пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int UpdateUser(User user)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Users
                    .Where(x => x.Id == user.Id)
                    .Set(x => x.Username, user.Username)
                    .Set(x => x.Email, user.Email)
                    .Update();
            }
        }

        /// <summary>
        /// Возвращает пользователя по почте
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override User GetUserByEmail(string email)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Users
                    .Where(x => x.Email == email)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Возвращает роль пользователя по его ид.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override string GetRoleNameByUserId(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from ro in database.Roles
                        join us in database.Users on ro.Id equals us.RoleId
                        select ro.Name)
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// Удаляет пользователя из базы данных.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override int DeleteUserById(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Users
                    .Where(x => x.Id == id)
                    .Delete();
            }
        }
        #endregion


        #region PostsProcessing
        /// <summary>
        /// Возвращает пост по ид
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override Post GetPostById(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Постранично возвращает посты
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override IEnumerable<Post> GetPosts(int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .Skip((page - 1) * perPage)
                    .Take(perPage)
                    .ToArray();
            }
        }

        /// <summary>
        /// Возвращает посты по категориям
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override IEnumerable<Post> GetPostsByCatgeryId(Guid id, int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from po in database.Posts
                        join ca in database.PostsCategories on po.Id equals ca.PostId
                        where ca.CategoryId == id
                        select po)
                        .Skip((page - 1) * perPage)
                        .Take(perPage)
                        .ToArray();
            }
        }

        /// <summary>
        /// Возвращает посты пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override IEnumerable<Post> GetPostsByUserId(Guid id, int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from po in database.Posts
                        where po.OwnerUserId == id
                        select po)
                        .Skip((page - 1) * perPage)
                        .Take(perPage)
                        .ToArray();
            }
        }

        /// <summary>
        /// Возвращает неактивные посты
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override IEnumerable<Post> GetInactivePosts(int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from po in database.Posts
                        where po.StatusCode == (int)StatusCodes.Inactive
                        select po)
                        .Skip((page - 1) * perPage)
                        .Take(perPage)
                        .ToArray();
            }
        }

        /// <summary>
        /// Добавляет новый пост
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public override int AddPost(Post post)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .Value(x => x.Id, Guid.NewGuid())
                    .Value(x => x.CreationDate, DateTime.Now)
                    .Value(x => x.ClosedDate, default(DateTime))
                    .Value(x => x.StatusCode, (int)StatusCodes.Active)
                    .Value(x => x.ViewsCount, 0)
                    .Value(x => x.Title, post.Title)
                    .Value(x => x.Content, post.Content)
                    .Value(x => x.OwnerUserId, post.OwnerUserId)
                    .Insert();
            }
        }

        /// <summary>
        /// Обновляет существующий пост
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public override int UpdatePost(Post post)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .Where(x => x.Id == post.Id)
                    .Set(x => x.Title, post.Title)
                    .Set(x => x.Content, post.Content)
                    .Update();
            }
        }

        /// <summary>
        /// Деактивирует пост
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override int DeactivatePostById(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .Where(x => x.Id == id)
                    .Set(x => x.StatusCode, (int)StatusCodes.Inactive)
                    .Update();
            }
        }
        #endregion
    }
}
