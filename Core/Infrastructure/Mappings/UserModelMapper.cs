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
    public class UserModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            MapUser(modelBuilder.Entity<User>());
        }

        private void MapUser(EntityTypeConfiguration<User> user)
        {
            user.HasKey(i => i.Id).ToTable("Users");

            user.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            user.Property(i => i.UserName).IsRequired();

            user.Property(i => i.FirstName).IsRequired();

            user.Property(i => i.LastName).IsRequired();

            user.Property(i => i.Email).IsRequired();

            user.Property(i => i.BrokerName).IsOptional();

            user.Property(i => i.BrokerDescription).IsOptional();

            user.Property(i => i.Password).IsRequired();

            user.HasMany(i => i.Roles).WithMany(m => m.Users).Map(m => m.MapLeftKey("FKUserID").MapRightKey("FKRoleID").ToTable("UserRoles"));

            user.Property(i => i.InsertDate).IsRequired();
        }
    }
}
