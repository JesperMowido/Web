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
    public class PlaceModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapPlace(modelBuilder.Entity<Place>());
        }

        private void MapPlace(EntityTypeConfiguration<Place> place)
        {
            place.HasKey(i => i.Id).ToTable("Places");

            place.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            place.Property(i => i.ParentId).HasColumnName("FKParentID").IsOptional();

            place.Property(i => i.PlaceTypeId).HasColumnName("FKPlaceTypeID").IsRequired();

            place.HasRequired(i => i.PlaceType).WithMany().HasForeignKey(i => i.PlaceTypeId);

            place.Property(i => i.Name).IsRequired();

            place.Property(i => i.Lat).IsOptional();

            place.Property(i => i.Long).IsOptional();
        }
    }
}
