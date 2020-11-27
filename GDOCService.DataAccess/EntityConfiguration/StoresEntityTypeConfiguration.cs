using System;
using System.Collections.Generic;
using System.Text;
using GDOCService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GDOCService.DataAccess.EntityConfiguration
{
    class StoresEntityTypeConfiguration : IEntityTypeConfiguration<Stores>
    {
        public void Configure(EntityTypeBuilder<Stores> builder)
        {
            builder.ToTable("Stores");
            builder.HasKey(x => new { x.StoreId, x.CompanyID, x.StoreCode });
        }
    }
}
