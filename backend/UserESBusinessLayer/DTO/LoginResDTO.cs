using System;
using System.Collections.Generic;
using System.Text;

namespace UserESBusinessLayer.DTO
{
    public class LoginResDTO
    {
        public string? Token { get; set; }  
        public bool IsAuthenticated { get; set; }   
        public string? Message { get; set; }    
    }
}
