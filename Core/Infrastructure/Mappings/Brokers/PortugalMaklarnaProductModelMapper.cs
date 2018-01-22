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
    public class PortugalMaklarnaProductModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapPortugalMaklarna(modelBuilder.Entity<PortugalMaklarnaProduct>());
        }

        private void MapPortugalMaklarna(EntityTypeConfiguration<PortugalMaklarnaProduct> broker)
        {
            broker.HasKey(i => i.Id).ToTable("PortugalMaklarnaProducts");

            broker.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            broker.Property(i => i.Price).IsOptional();

            broker.Property(i => i.City).IsOptional();

            broker.Property(i => i.Area).IsOptional();

            broker.Property(i => i.LivingSpace).IsOptional();

            broker.Property(i => i.NumberOfRooms).IsOptional();

            broker.Property(i => i.ImageUrl1).IsOptional();

            broker.Property(i => i.ImageUrl2).IsOptional();

            broker.Property(i => i.ImageUrl3).IsOptional();

            broker.Property(i => i.ImageUrl4).IsOptional();

            broker.Property(i => i.ImageUrl5).IsOptional();

            broker.Property(i => i.Description).IsOptional();

            broker.Property(i => i.Address).IsOptional();

            broker.Property(i => i.ExternalLink).IsOptional();

            broker.Property(i => i.RefNo).IsOptional();

            broker.Property(i => i.YearBuilt).IsOptional();
        }
    }
}
