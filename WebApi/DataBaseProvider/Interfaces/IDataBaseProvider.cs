using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.DataBaseProvider.Interfaces
{
    public interface IDataBaseProvider
    {
        bool AddUser(User user);
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        IEnumerable<User> GetUsers();
        IEnumerable<Role> GetRoles();
        Role GetRoleById(Guid id);
    }
}
