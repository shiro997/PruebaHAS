using System;
using System.Collections.Generic;
using System.Text;
using UserESDataLayer.Model;

namespace UserESDataLayer
{
    public interface IUserESData
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> CreateUserAsync(User user);
        public Task<bool> UpdateUserAsync(User user);
        public Task<bool> DeleteUserAsync(int id);
    }
}
