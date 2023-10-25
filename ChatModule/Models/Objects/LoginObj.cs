using System.ComponentModel.DataAnnotations;

namespace Models.Objects
{
    public class LoginObj
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
