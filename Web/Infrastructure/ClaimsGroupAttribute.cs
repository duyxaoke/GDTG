using System;
using System.ComponentModel.DataAnnotations;


namespace Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ClaimsGroupAttribute : Attribute
    {
        public ClaimResources Resource { get; private set; }

        public ClaimsGroupAttribute(ClaimResources resource)
        {
            Resource = resource;
        }

        public String GetGroupId()
        {
            return ((int)Resource).ToString();
        }
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ClaimsActionAttribute : Attribute
    {
        public ClaimsActions Claim { get; private set; }

        public ClaimsActionAttribute(ClaimsActions claim)
        {
            Claim = claim;
        }
    }

    public enum ClaimsActions
    {
        Index,
        View,
        Create,
        Edit,
        Delete,
        HolidayRequest
    }


    public enum ClaimResources
    {
        [Display(Name = "Menus")]
        Menus = 1,
        [Display(Name = "Roles")]
        Roles = 2,
        [Display(Name = "Users")]
        Users = 3,
        [Display(Name = "Categories")]
        Categories = 4,
        [Display(Name = "Products")]
        Products = 5,
        [Display(Name = "Specification")]
        Specification = 6,
        [Display(Name = "Manufacturer")]
        Manufacturer = 7,
        [Display(Name = "ImageManager")]
        ImageManager = 8,
    }
}