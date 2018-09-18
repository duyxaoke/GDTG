using System;
using Data.Models;
using Microsoft.Owin.Security;

namespace Data.Models
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<ApplicationUser, String>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}
