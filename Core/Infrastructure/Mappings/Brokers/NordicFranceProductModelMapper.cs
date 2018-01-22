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
    public class NordicFranceProductModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapBroker(modelBuilder.Entity<NordicFranceProduct>());
        }

        private void MapBroker(EntityTypeConfiguration<NordicFranceProduct> broker)
        {
            broker.HasKey(i => i.Id).ToTable("NordicFranceProducts");

            broker.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            broker.Property(i => i.Price).IsOptional();

            broker.Property(i => i.City).IsOptional();

            broker.Property(i => i.Area).IsOptional();

            broker.Property(i => i.RefNo).IsOptional();

            broker.Property(i => i.ImageUrl1).IsOptional();

            broker.Property(i => i.ImageUrl2).IsOptional();

            broker.Property(i => i.ImageUrl3).IsOptional();

            broker.Property(i => i.ImageUrl4).IsOptional();

            broker.Property(i => i.ImageUrl5).IsOptional();

            broker.Property(i => i.Desc1).IsOptional();

            broker.Property(i => i.Desc2).IsOptional();

            broker.Property(i => i.Address).IsOptional();

            broker.Property(i => i.Address2).IsOptional();

            broker.Property(i => i.PriceEUR).IsOptional();

            broker.Property(i => i.ExternalLink).IsOptional();
        }
    }
}
