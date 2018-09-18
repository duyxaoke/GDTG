using System;
namespace Core.Data
{
    public class MenuInRoles
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }
        public int MenuId { get; set; }
    }
}
