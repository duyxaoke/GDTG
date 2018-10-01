using Core.Data;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mapping
{
    public class MenuInRolesMap : EntityTypeConfiguration<MenuInRoles>
    {
        public MenuInRolesMap()
        {
            //key
            HasKey(t => t.Id);
            //table
            ToTable("MenuInRoles");
        }
    }
}