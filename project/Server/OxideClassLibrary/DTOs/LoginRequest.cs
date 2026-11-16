using System;
using System.Collections.Generic;
using System.Text;

namespace OxideClassLibrary.DTOs
{
    public class LoginRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
