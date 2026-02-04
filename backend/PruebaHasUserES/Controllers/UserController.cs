using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using UserESBusinessLayer;
using UserESBusinessLayer.DTO;

namespace PruebaHasUserES.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _conf;
        private readonly IUserESImpl _userESImpl;
        private readonly ILogger _logger;

        public UserController(IConfiguration conf)
        {
            _logger = LoggerFactory.Create(builder => 
            {
                builder.AddConsole();
            }).CreateLogger<UserController>();
            _conf = conf;
            
            _userESImpl = new UserEsImpl(_conf);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userESImpl.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Get Users: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userESImpl.GetUserByIdAsync(id);
                
                return Ok(user);
            }
            catch (NullReferenceException NE) 
            {
                _logger.LogError($"Null Reference Error in Get User By Id: {NE.Message}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Get User By Id: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user) 
        {
            try
            {
                UserDTO createdUser = await _userESImpl.CreateUserAsync(user);

                return Ok(createdUser);
            }
            catch (NullReferenceException NE)
            { 
                _logger.LogError($"Error Creating User: {NE.Message}");
                return BadRequest(NE.Message);
            }
            catch (ArgumentException AE)
            {
                _logger.LogError($"Argument Error Creating User: {AE.Message}");
                return BadRequest(AE.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Creating User: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user) 
        {
            try
            {
                bool isUpdated =  await _userESImpl.UpdateUserAsync(user);

                if (!isUpdated) 
                {
                    return NotFound("User not found");
                }

                return Ok("User updated successfully");

            }
            catch (NullReferenceException NE) 
            {
                _logger.LogError($"Error Updating User: {NE.Message}");
                return NotFound(NE.Message);
            }
            catch (ArgumentException AE)
            {
                _logger.LogError($"Argument Error Updating User: {AE.Message}");
                return BadRequest(AE.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating User: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            try 
            {
                bool isDeleted = await _userESImpl.DeleteUserAsync(id);
                if (!isDeleted) 
                {
                    return NotFound("User not found");
                }
                return Ok("User deleted successfully");
            }
            catch (NullReferenceException NE)
            {
                _logger.LogError($"Error Updating User: {NE.Message}");
                return NotFound(NE.Message);
            }
            catch (ArgumentException AE)
            {
                _logger.LogError($"Argument Error Updating User: {AE.Message}");
                return BadRequest(AE.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating User: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO loginReq)
        {
            try
            {
                LoginResDTO loginRes = await _userESImpl.LoginAsync(loginReq);
                if (!loginRes.IsAuthenticated)
                {
                    return Unauthorized(loginRes.Message);
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Protege contra XSS
                    Secure = true,   // Solo envía sobre HTTPS
                    SameSite = SameSiteMode.Lax, // Previene CSRF
                    Expires = DateTime.UtcNow.AddMinutes(30) // Coincidir con el token
                };

                Response.Cookies.Append("AuthToken", loginRes.Token, cookieOptions);

                return Ok(loginRes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Login: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
