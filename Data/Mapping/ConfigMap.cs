using Core.Data;

namespace Data.Mapping
{
    public class ConfigMap : EntityTypeConfiguration<Config>
    {
        public ConfigMap()
        {
            //key
            HasKey(t => t.Id);
            //table
            ToTable("Config");
        }
    }
}