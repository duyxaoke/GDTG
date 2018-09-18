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
   public class MenuMap : EntityTypeConfiguration<Menu> 
    {
       public MenuMap()
       {
            //key
            HasKey(t => t.Id);
           Property(t => t.Name).HasMaxLength(100).HasColumnType("nvarchar");
           Property(t => t.Icon).HasMaxLength(20).HasColumnType("nvarchar");
           Property(t => t.Url).HasMaxLength(100).HasColumnType("nvarchar");
            //table
            ToTable("Menu");
       }
    }
}
