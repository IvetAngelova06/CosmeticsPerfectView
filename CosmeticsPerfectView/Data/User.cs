using Microsoft.AspNetCore.Identity;

namespace CosmeticsPerfectView.Data
{
    public class User:IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }   
    }
}