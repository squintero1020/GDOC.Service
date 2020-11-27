using System;
using System.Collections.Generic;
using System.Text;
using GDOCService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GDOCService.DataAccess.EntityConfiguration
{
    class GunsEntityTypeConfiguration : IEntityTypeConfiguration<Guns>
    {
        public void Configure(EntityTypeBuilder<Guns> builder)
        {
            builder.ToTable("Guns");
            builder.HasKey(x => new { x.Gunsid });
        }
    }
}
