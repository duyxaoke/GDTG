using System.Collections.Generic;
using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(MenuViewModelValidator))]
    public class MenuViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Guid? RoleId { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public bool Checked { get; set; } // in role - checked
        public string RoleName { get; set; } // in role - checked
        [NotMapped]
        public IEnumerable<SelectListViewModel> Parents { get; set; }
        [NotMapped]
        public IEnumerable<MenuViewModel> Childrens { get; set; }
        public List<int> MenuIds { get; set; }

    }
    public class MenuViewModelValidator : AbstractValidator<MenuViewModel>
    {
        public MenuViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 50);
            RuleFor(x => x.Url).Length(0, 255);
            RuleFor(x => x.Icon).Length(0, 255);
            RuleFor(x => x.RoleName).Length(0, 255);
        }
    }

}
