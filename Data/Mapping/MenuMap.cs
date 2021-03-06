﻿using Core.Data;
using System.Data.Entity.ModelConfiguration;

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