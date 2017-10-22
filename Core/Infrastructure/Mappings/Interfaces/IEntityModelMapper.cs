using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Mappings.Interfaces
{
    public interface IEntityModelMapper
    {
        void Map(DbModelBuilder modelBuilder);
    }
}
