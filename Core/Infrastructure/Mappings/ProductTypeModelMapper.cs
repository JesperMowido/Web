using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Mappings
{
    public class ProductTypeModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapProductType(modelBuilder.Entity<ProductType>());
        }

        private void MapProductType(EntityTypeConfiguration<ProductType> product)
        {
            product.HasKey(i => i.Id).ToTable("ProductTypes");

            product.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            product.Property(i => i.Name).IsRequired();
        }
    }
}
