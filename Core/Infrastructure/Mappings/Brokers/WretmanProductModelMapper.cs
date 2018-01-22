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
    public class WretmanProductModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapBroker(modelBuilder.Entity<WretmanProduct>());
        }

        private void MapBroker(EntityTypeConfiguration<WretmanProduct> broker)
        {
            broker.HasKey(i => i.Id).ToTable("WretmanProducts");

            broker.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            broker.Property(i => i.Price).IsOptional();

            broker.Property(i => i.Place).IsOptional();

            broker.Property(i => i.Spaces).IsOptional();

            broker.Property(i => i.MonthlyPrice).IsOptional();

            broker.Property(i => i.ImageUrl1).IsOptional();

            broker.Property(i => i.ImageUrl2).IsOptional();

            broker.Property(i => i.ImageUrl3).IsOptional();

            broker.Property(i => i.ImageUrl4).IsOptional();

            broker.Property(i => i.ImageUrl5).IsOptional();

            broker.Property(i => i.Description).IsOptional();

            broker.Property(i => i.Address).IsOptional();

            broker.Property(i => i.ExternalLink).IsOptional();
        }
    }
}
