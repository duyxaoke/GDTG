using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public class UpdateMenuInRoleViewModel
    {
        public Guid RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}