using System.ComponentModel.DataAnnotations;

namespace UserESBusinessLayer.DTO
{
    public class UserDTO
    {
        public int IdUsuario { get; set; }
        
        public string? NombreUsuario { get; set; }
        
        public string? Email { get; set; }
        
        public string? UsrPassword { get; set; }
    }
}
