using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data.Common;
using UserESBusinessLayer.DTO;
using UserESDataLayer;
using UserESDataLayer.Model;

namespace UserESBusinessLayer
{
    public class UserEsImpl
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        private IUserESData _userData;

        public UserEsImpl(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _userData = new UserESData(_configuration, _logger);
        }

        private string HashAndSaltPassword(string password)
        {
            var options = Options.Create(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                IterationCount = 3100000
            });
            var passwordHasher = new PasswordHasher<string>(options);
            string passwordCyphered = passwordHasher.HashPassword(null, password);
            return passwordCyphered;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            try
            {
                List<UserDTO> userDTOs = new List<UserDTO>();
                IEnumerable<User> usersFromDb = await _userData.GetUsersAsync();
                if (usersFromDb == null)
                {
                    throw new NullReferenceException(nameof(usersFromDb));
                }
                foreach (User user in usersFromDb)
                {
                    UserDTO userDTO = new UserDTO
                    {
                        IdUsuario = user.IdUsuario,
                        NombreUsuario = user.NombreUsuario,
                        Email = user.Email
                    };
                    userDTOs.Add(userDTO);
                }
                return userDTOs;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "No users were found in the database or the process was unsuccesful.");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was canceled while retrieving users.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving users.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving users.");
                throw;
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                User userFromDb = await _userData.GetUserByIdAsync(id);
                if (userFromDb == null)
                {
                    throw new NullReferenceException(nameof(userFromDb));
                }
                UserDTO userDTO = new UserDTO
                {
                    IdUsuario = userFromDb.IdUsuario,
                    NombreUsuario = userFromDb.NombreUsuario,
                    Email = userFromDb.Email
                };
                return userDTO;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, $"User with ID {id} was not found in the database.");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was canceled while retrieving user by ID.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving user by ID.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user by ID.");
                throw;
            }
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO user)
        {
            try
            {
                if (user.Equals(null))
                {
                    throw new ArgumentNullException(nameof(user), "The user can't be null");
                }
                User newUser = new User
                {
                    NombreUsuario = user.NombreUsuario,
                    Email = user.Email,
                    UsrPassword = HashAndSaltPassword(user.UsrPassword ?? string.Empty)
                };
                User createdUser = await _userData.CreateUserAsync(newUser);
                UserDTO createdUserDTO = new UserDTO
                {
                    IdUsuario = createdUser.IdUsuario,
                    NombreUsuario = createdUser.NombreUsuario,
                    Email = createdUser.Email
                };
                return createdUserDTO;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "The user provided for creation is null.");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was canceled while creating a new user.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating a new user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new user.");
                throw;
            }
        }
    }
}
