using Core.Data;

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