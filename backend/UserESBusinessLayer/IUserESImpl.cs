using System;
using System.Collections.Generic;
using System.Text;
using UserESBusinessLayer.DTO;

namespace UserESBusinessLayer
{
    public interface IUserESImpl
    {
        public Task<IEnumerable<UserDTO>> GetUsersAsync();
        public Task<UserDTO> GetUserByIdAsync(int id);
        public Task<UserDTO> CreateUserAsync(UserDTO user);
        public Task<bool> UpdateUserAsync(UserDTO user);
        public Task<bool> DeleteUserAsync(int id);
        public Task<LoginResDTO> LoginAsync(LoginReqDTO login);
    }
}
