using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shared.Models
{
    public class SelectListViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }
}
