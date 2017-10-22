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
    public class TestConnectionModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            MapTestConnection(modelBuilder.Entity<TestConnection>());
        }

        private void MapTestConnection(EntityTypeConfiguration<TestConnection> test)
        {
            test.HasKey(i => i.Id).ToTable("TestConnections");

            test.Property(i => i.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            test.Property(i => i.Name).IsRequired();
        }
    }
}
