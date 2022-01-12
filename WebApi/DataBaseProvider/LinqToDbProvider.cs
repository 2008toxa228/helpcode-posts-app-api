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
        private string _defaultRoleName = "User";

        #region Queries


        public override IEnumerable<object> Query1(string username, DateTime startDate, DateTime endDate)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                //return database.Posts.Where(x => x.CreationDate >= startDate && x.CreationDate < endDate)
                //    .Join(x)

                return (from po in database.Posts
                        join us in database.Users on po.OwnerUserId equals us.Id
                        where us.Username == username
                            && po.CreationDate >= startDate && po.CreationDate < endDate
                        select new { title = po.Title, content = po.Content.Substring(0, 100), creationDate = po.CreationDate, count = (from c2 in database.Comments where c2.PostId == po.Id group c2 by c2.Id into grp select grp.Count()) }).ToArray();
            }
        }
        public override IEnumerable<object> Query2(string username, DateTime startDate, DateTime endDate)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from po in database.Posts
                        join us in database.Users on po.OwnerUserId equals us.Id
                        join co in database.Comments on po.Id equals co.PostId
                        where us.Username == username
                            && po.CreationDate >= startDate && po.CreationDate < endDate
                        select new { title = po.Title, content = co.Content, co.CreationDate }).ToArray();
            }
        }
        public override IEnumerable<object> Query3(string postTitle)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from p in database.Posts
                        join c in database.Comments on p.Id equals c.PostId
                        join u in database.Users on c.OwnerUserId equals u.Id
                        where p.Title == postTitle
                        group new { p, u } by new { p.Id, p.CreationDate, u.Username }
                        into grp
                        select new { id = grp.Key.Id, date = grp.Key.CreationDate, username = grp.Key.Username, count = grp.Count() })
                        .ToArray();
            }
        }

        #endregion



        #region CommentsProcessing

        public override int AddComment(Comment comment)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Comments
                    .Value(x => x.Id, Guid.NewGuid())
                    .Value(x => x.PostId, comment.PostId)
                    .Value(x => x.OwnerUserId, comment.OwnerUserId)
                    .Value(x => x.StatusCode, (int)StatusCodes.Active)
                    .Value(x => x.Content, comment.Content)
                    .Value(x => x.CreationDate, DateTime.Now)
                    .Insert();
            }
        }
        public override IEnumerable<Comment> GetCommentsByPostId(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Comments
                    .Where(x => x.PostId == id)
                    .ToArray();
            }
        }

        #endregion



        #region CategoriesProcessing


        public override IEnumerable<Category> GetCategoriesByPostId(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return (from pc in database.PostsCategories
                        join c in database.Categories on pc.CategoryId equals c.Id
                        where pc.PostId == id
                        select c)
                        .ToArray();
            }
        }

        public override IEnumerable<Category> GetCategories(int page, int perPage = 100)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Categories
                    .Skip((page - 1) * perPage)
                    .Take(perPage)
                    .ToArray();
            }
        }

        public override int AddPostCategory(Guid postId, Guid categoryId)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                if (database.PostsCategories
                    .Where(x => x.PostId == postId && categoryId == categoryId)
                    .Any())
                {
                    return 0;
                }

                return database.PostsCategories
                    .Value(x => x.PostId, postId)
                    .Value(x => x.CategoryId, categoryId)
                    .Insert();
            }
        }
        public override int RemovePostCategory(Guid postId, Guid categoryId)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.PostsCategories
                    .Where(x => x.PostId == postId && categoryId == categoryId)
                    .Delete();
            }
        }

        public override int AddCategory(string name, string description)
        {

            using (var database = new HelpCodeDb(ConnectionString))
            {
                if (database.Categories.Where(x => x.Name == name).Any())
                {
                    return 0;
                }

                return database.Categories
                    .Value(x => x.Id, Guid.NewGuid())
                    .Value(x => x.StatusCode, (int)StatusCodes.Active)
                    .Value(x => x.Name, name)
                    .Value(x => x.Description, description)
                    .Insert();
            }
        }
        public override Category GetCategoryByName(string name)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Categories
                    .Where(x => x.Name == name)
                    .FirstOrDefault();
            }
        }
        public override Category GetCategoryById(Guid id)
        {
            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Categories
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }


        #endregion



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
                    .Where(x => x.Name == _defaultRoleName)
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

                if (database.Users.Where(x => x.Email == user.Email).Any())
                {
                    return 0;
                }

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
                        where us.Id == id
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
        public override IEnumerable<Post> GetPosts(int page = 1, int perPage = 100)
        {
            if (page < 1)
            {
                page = 1;
            }

            using (var database = new HelpCodeDb(ConnectionString))
            {
                return database.Posts
                    .OrderByDescending(x => x.CreationDate)
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
                        orderby po.CreationDate descending
                        select po
                        )
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
                        .OrderByDescending(x => x.CreationDate)
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
                        orderby po.CreationDate descending
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
                    .Value(x => x.ClosedDate, default(DateTime?))
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
