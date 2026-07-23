using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using UserESDataLayer.Model;

namespace UserESDataLayer
{
    public class UserESData : IUserESData
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        private string _connectionString;

        public UserESData(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            this._connectionString = _configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                List<User> users = new List<User>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT IdUsuario, NombreUsuario, Email, UsrPassword FROM Users";
                        cmd.CommandType = CommandType.Text;
                        await cmd.Connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                User user = new User()
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    NombreUsuario = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    UsrPassword = reader.GetString(3)
                                };
                                users.Add(user);
                            }
                        }
                        return users;
                    }
                }
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

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                User user = null;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT IdUsuario, NombreUsuario, Email, UsrPassword FROM Users WHERE IdUsuario = @IdUsuario";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        await cmd.Connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new User()
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    NombreUsuario = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    UsrPassword = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
                return user;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, $"Operation was canceled while retrieving user with ID {id}.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Database error occurred while retrieving user with ID {id}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving user with ID {id}.");
                throw;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO Users (NombreUsuario, Email, UsrPassword) OUTPUT INSERTED.IdUsuario VALUES (@NombreUsuario, @Email, @UsrPassword)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@NombreUsuario", user.NombreUsuario);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@UsrPassword", user.UsrPassword);
                        await cmd.Connection.OpenAsync();
                        user.IdUsuario = (int)await cmd.ExecuteScalarAsync();
                    }
                }
                return user;
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

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                int rowsAffected = 0;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "UPDATE Users SET NombreUsuario = @NombreUsuario, Email = @Email, UsrPassword = @UsrPassword WHERE IdUsuario = @IdUsuario";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", user.IdUsuario);
                        cmd.Parameters.AddWithValue("@NombreUsuario", user.NombreUsuario);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@UsrPassword", user.UsrPassword);
                        await cmd.Connection.OpenAsync();
                        rowsAffected = await cmd.ExecuteNonQueryAsync();
                    }
                }
                return rowsAffected > 0;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, $"Operation was canceled while updating user with ID {user.IdUsuario}.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Database error occurred while updating user with ID {user.IdUsuario}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating user with ID {user.IdUsuario}.");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                int rowsAffected = 0;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "DELETE FROM Users WHERE IdUsuario = @IdUsuario";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        await cmd.Connection.OpenAsync();
                        rowsAffected = await cmd.ExecuteNonQueryAsync();
                    }
                }
                return rowsAffected > 0;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, $"Operation was canceled while deleting user with ID {id}.");
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Database error occurred while deleting user with ID {id}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting user with ID {id}.");
                throw;
            }
        }

    }
}
