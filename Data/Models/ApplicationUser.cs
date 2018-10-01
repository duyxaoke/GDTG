using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        public decimal Balance { get; set; }
    }
}