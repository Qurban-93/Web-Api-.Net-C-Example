using Microsoft.AspNetCore.Identity;

namespace FirstAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
