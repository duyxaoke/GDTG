using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shared.Models
{
    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel()
        {
            ClaimGroups = new List<ClaimGroup>();

            SelectedClaims = new List<String>();
        }

        public String RoleId { get; set; }

        public String RoleName { get; set; }

        public List<ClaimGroup> ClaimGroups { get; set; }

        public IEnumerable<String> SelectedClaims { get; set; }


        public class ClaimGroup
        {
            public ClaimGroup()
            {
                GroupClaimsCheckboxes = new List<SelectListViewModel>();
            }
            public String GroupName { get; set; }

            public int GroupId { get; set; }

            public List<SelectListViewModel> GroupClaimsCheckboxes { get; set; }
        }
    }
}
