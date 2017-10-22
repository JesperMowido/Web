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
    public class PlaceTypeModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapPlaceType(modelBuilder.Entity<PlaceType>());
        }

        private void MapPlaceType(EntityTypeConfiguration<PlaceType> place)
        {
            place.HasKey(i => i.Id).ToTable("PlaceTypes");

            place.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            place.Property(i => i.Name).IsRequired();
        }
    }
}
