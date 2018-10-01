using Microsoft.Owin.Security;
using System;

namespace Data.Models
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<ApplicationUser, String>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}