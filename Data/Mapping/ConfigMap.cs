using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
