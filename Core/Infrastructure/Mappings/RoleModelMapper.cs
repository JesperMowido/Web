using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Mappings
{
    public class RoleModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            MapRole(modelBuilder.Entity<Role>());
        }

        private void MapRole(EntityTypeConfiguration<Role> role)
        {
            role.HasKey(i => i.Id).ToTable("Roles");

            role.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            role.Property(i => i.Name).IsRequired();

            role.HasMany(t => t.Users).WithMany().Map(m => m.MapLeftKey("FKRoleID").MapRightKey("FKUserID").ToTable("UserRoles"));
        }
    }
}
