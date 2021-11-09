using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataBaseProvider.Interfaces;
using WebApi.Entities;
using WebApi.Infrastructure.Hash;
using WebApi.Models;

namespace WebApi.DataBaseProvider
{
    public class MockDataBaseProvider : IDataBaseProvider
    {
        private static List<User> Users = new List<User>()
        {
            //passwords is "123"
            new User("{\"Id\":\"c458690d-7a08-4d37-978d-120d6a62a0b4\",\"RoleId\":\"858dcdcd-7093-4294-aa85-db86902a3d90\",\"Email\":\"e\",\"Username\":\"user\",\"Reputation\":0,\"PasswordHash\":\"BF85532CFEEC51F06F2ADD4F4F684335CE10A6F25F5B3AA19BA180CA875F4F2BB382883903B92CBA8EF1B0442B7590503DA50223DD433099EE230ADE2404B068\"}"),
            new User("{\"Id\":\"62aaaf41-f9e8-4c4f-990b-d93ed5cb11bf\",\"RoleId\":\"858dcdcd-7093-4294-aa85-db86902a3d90\",\"Email\":\"asd\",\"Username\":\"user\",\"Reputation\":0,\"PasswordHash\":\"BF85532CFEEC51F06F2ADD4F4F684335CE10A6F25F5B3AA19BA180CA875F4F2BB382883903B92CBA8EF1B0442B7590503DA50223DD433099EE230ADE2404B068\"}"),
        };
        private static List<Role> Roles = new List<Role>()
        {
            new Role("{\"Id\":\"858dcdcd-7093-4294-aa85-db86902a3d90\",\"Name\":\"default\",\"Description\":\"role by default\"}"),
            new Role("{\"Id\":\"e6e15c81-7f54-451d-b965-ea26902053ef\",\"Name\":\"admin\",\"Description\":\"admin role\"}"),
        };
        private static List<RefreshToken> RefreshTokens = new List<RefreshToken>()
        {

        };

        public Role GetRoleById(Guid id)
        {
            return Roles.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Role> GetRoles()
        {
            return Roles.ToArray();
        }
        public User GetUserById(Guid id)
        {
            return Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return Users.Where(x => x.Email == email).FirstOrDefault();
        }


        public User GetUserByUsername(string username)
        {

            return Users.Where(x => x.Username == username).FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return Users.ToArray();
        }

        public bool UpdateRefreshToken(RefreshToken model)
        {
            try
            {
                RefreshTokens.RemoveAll(x => x.UserId == model.UserId);

                RefreshTokens.Add(model);

                return true;
            }
            catch (Exception e)
            {
                // ToDo log exception
                return false;
            }
        }
        public RefreshToken GetRefreshToken(Guid userId)
        {
            var token = RefreshTokens.Where(x => x.UserId == userId).FirstOrDefault();

            return token;
        }

        public bool AddUser(User user)
        {
            if (Users.Where(x => x.Email == user.Email || x.Id == user.Id).FirstOrDefault() != null)
            {
                return false;
            }

            Users.Add(user);

            return true;
        }
    }
}
