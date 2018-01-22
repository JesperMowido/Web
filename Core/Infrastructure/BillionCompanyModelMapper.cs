using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Core.Infrastructure.Mappings.Interfaces;
using Core.Infrastructure.Mappings;

namespace Core.Infrastructure
{
    public abstract class IBillionCompanyMapperBase : IEntityModelMapper
    {
        public abstract void Map(DbModelBuilder modelBuilder);
    }

    public class BillionCompanyModelMapper : IBillionCompanyMapperBase
    {
        public override void Map(DbModelBuilder modelBuilder)
        {
            ExecuteModelMapper<UserModelMapper>(modelBuilder);
            ExecuteModelMapper<RoleModelMapper>(modelBuilder);
            ExecuteModelMapper<TestConnectionModelMapper>(modelBuilder);
            ExecuteModelMapper<PlaceTypeModelMapper>(modelBuilder);
            ExecuteModelMapper<PlaceModelMapper>(modelBuilder);
            ExecuteModelMapper<ProductTypeModelMapper>(modelBuilder);
            ExecuteModelMapper<ProductModelMapper>(modelBuilder);

            ////////// BROKERS IMPORT ///////////
            ExecuteModelMapper<SkandiaProductModelMapper>(modelBuilder);
            ExecuteModelMapper<WretmanProductModelMapper>(modelBuilder);
            ExecuteModelMapper<PortugalMaklarnaProductModelMapper>(modelBuilder);
            ExecuteModelMapper<NordicFranceProductModelMapper>(modelBuilder);
            ExecuteModelMapper<CCHomesProductModelMapper>(modelBuilder);
        }

        private void ExecuteModelMapper<TModelMapper>(DbModelBuilder modelBuilder) where TModelMapper : IBillionCompanyMapperBase, new()
        {
            TModelMapper modelMapper = new TModelMapper();
            modelMapper.Map(modelBuilder);
        }
    }
}
