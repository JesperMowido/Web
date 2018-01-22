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
    public class SkandiaProductModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapPlace(modelBuilder.Entity<SkandiaProduct>());
        }

        private void MapPlace(EntityTypeConfiguration<SkandiaProduct> place)
        {
            place.HasKey(i => i.Id).ToTable("SkandiaProducts");

            place.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            place.Property(i => i.Price).IsOptional();

            place.Property(i => i.Place).IsOptional();

            place.Property(i => i.LivingSpace).IsOptional();

            place.Property(i => i.NumberOfRooms).IsOptional();

            place.Property(i => i.ImageUrl1).IsOptional();

            place.Property(i => i.ImageUrl2).IsOptional();

            place.Property(i => i.ImageUrl3).IsOptional();

            place.Property(i => i.ImageUrl4).IsOptional();

            place.Property(i => i.ImageUrl5).IsOptional();

            place.Property(i => i.Description).IsOptional();

            place.Property(i => i.Address).IsOptional();

            place.Property(i => i.ExternalLink).IsOptional();

            place.Property(i => i.RefNo).IsOptional();
        }
    }
}
