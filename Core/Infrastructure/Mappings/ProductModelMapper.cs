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
    public class ProductModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapProduct(modelBuilder.Entity<Product>());
        }

        private void MapProduct(EntityTypeConfiguration<Product> product)
        {
            product.HasKey(i => i.Id).ToTable("Products");

            product.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            product.Property(i => i.ProductTypeId).HasColumnName("FKProductTypeID").IsRequired();

            product.HasRequired(i => i.ProductType).WithMany().HasForeignKey(i => i.ProductTypeId);

            product.Property(i => i.SalesResponsibleId).HasColumnName("SalesResponsibleID").IsRequired();

            product.HasRequired(i => i.SalesResponsible).WithMany().HasForeignKey(i => i.SalesResponsibleId);

            product.Property(i => i.PlaceId).HasColumnName("FKPlaceID").IsRequired();

            product.HasRequired(i => i.Place).WithMany().HasForeignKey(i => i.PlaceId);

            product.Property(i => i.Description).IsRequired();

            product.Property(i => i.LivingSpace).IsOptional();

            product.Property(i => i.PlotSize).IsOptional();

            product.Property(i => i.NumberOfRooms).IsOptional();

            product.Property(i => i.Price).IsOptional();

            product.Property(i => i.MonthlyCharge).IsOptional();

            product.Property(i => i.SquareMeterPrice).IsOptional();

            product.Property(i => i.YearBuilt).IsOptional();

            product.Property(i => i.ExternalLink).IsRequired();

            product.Property(i => i.ExternalPicLink).IsOptional();

            product.Property(i => i.InsertDate).IsRequired();

            product.Property(i => i.Address).IsRequired();

            product.Property(i => i.Lat).IsOptional();

            product.Property(i => i.Long).IsOptional();

            product.Property(i => i.IsApproved).IsRequired();

            product.Property(i => i.RefNr).IsOptional();

            product.Property(i => i.ThumbnailUrl).IsOptional();

            product.Property(i => i.PriceFilter).IsOptional();
        }
    }
}
