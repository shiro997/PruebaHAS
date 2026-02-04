using System.ComponentModel.DataAnnotations;

namespace PruebaHasUserES.DTO
{
    public class UserDTO
    {
        public int IdUsuario { get; set; }
        [Required]
        public required string NombreUsuario { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string UsrPassword { get; set; }
    }
}
