using Core.Data;
using System.Data.Entity.ModelConfiguration;

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