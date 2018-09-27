using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public String Name { get; set; }
    }
}