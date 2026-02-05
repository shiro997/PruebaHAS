using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data.Common;
using UserESBusinessLayer.DTO;
using UserESDataLayer;
using UserESDataLayer.Model;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;

namespace UserESBusinessLayer
{
    public class UserEsImpl : IUserESImpl
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        private IUserESData _userData;

        public UserEsImpl(IConfiguration configuration)
        {
            _logger = LoggerFactory.Create(builder => 
            {
                builder.AddConsole();
            }).CreateLogger<UserEsImpl>();
            _configuration = configuration;
            
            _userData = new UserESData(_configuration);
        }

        private string HashAndSaltPassword(string password)
        {
            var options = Options.Create(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                IterationCount = 310000
            });
            var passwordHasher = new PasswordHasher<string>(options);
            string passwordCyphered = passwordHasher.HashPassword(null, password);
            return passwordCyphered;
        }

        private bool ComparePasswords(string hashedPassword, string providedPassword) 
        {
            var options = Options.Create(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                IterationCount = 310000
            });
            var passwordHasher = new PasswordHasher<string>(options);
            var verificated = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return verificated == PasswordVerificationResult.Success;
        }

        private string GenerateAccessToken(string Email) 
        {
            string Key = _configuration.GetValue<string>("JWT:Key");
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(Key);

            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Email));

            SigningCredentials tokenCred = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor desc = new SecurityTokenDescriptor{ 
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration.GetValue<string>("JWT:ValidIssuer"),
                SigningCredentials = tokenCred
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(desc);

            string tokenCreated = tokenHandler.WriteToken(tokenConfig);
            return tokenCreated;

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
                    UsrPassword = HashAndSaltPassword(user.UsrPassword)
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

        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            try
            {
                if (user.Equals(null))
                {
                    throw new ArgumentNullException(nameof(user), "The user can't be null");
                }
                User userToUpdate = new User
                {
                    IdUsuario = user.IdUsuario,
                    NombreUsuario = user.NombreUsuario,
                    Email = user.Email,
                    UsrPassword = user.UsrPassword == null || user.UsrPassword == "" ? "" : HashAndSaltPassword(user.UsrPassword)
                };
                bool updateResult = await _userData.UpdateUserAsync(userToUpdate);
                return updateResult;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "The user provided for update is null.");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was canceled while updating a user.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating a user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating a user.");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                if (id < 0) 
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "The user ID must be a non-negative integer.");
                }
                if (id == 0) 
                {
                    throw new ArgumentException("The user ID cannot be zero.", nameof(id));
                }
                bool deleteResult = await _userData.DeleteUserAsync(id);
                return deleteResult;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "The user ID provided for deletion is out of range.");
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "The user ID provided for deletion is invalid.");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was canceled while deleting a user.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting a user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting a user.");
                throw;
            }
        }

        public async Task<LoginResDTO> LoginAsync(LoginReqDTO login) 
        {
            try 
            {
                var Users = await _userData.GetUsersAsync();
                var user = Users.FirstOrDefault(u => u.Email == login.Email);
                if (user == null) 
                {
                    return new LoginResDTO 
                    {
                        IsAuthenticated = false,
                        Message = "User not found."
                    };
                }

                string hashedPassword = user.UsrPassword;
                
                bool isVerificated = ComparePasswords(hashedPassword, login.Password);
                if (!isVerificated) 
                {
                    throw new Exception("Invalid Password");
                }
                string token = GenerateAccessToken(user.Email);

                return new LoginResDTO
                {
                    IsAuthenticated = true,
                    Token = token,
                    Message = "Login successful.",
                    User = new UserDTO
                    {
                        IdUsuario = user.IdUsuario,
                        NombreUsuario = user.NombreUsuario,
                        Email = user.Email
                    }
                };

            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "The process was unsuccesful.");
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
            catch (Exception e) 
            {
                _logger.LogError(e, "An unexpected error occurred during login.");
                throw;
            }
        }
    }
}
