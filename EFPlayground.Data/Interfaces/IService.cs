using EFPlayground.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Data
{
    public interface IService<TEntity> where TEntity : ModelBase
    {
        TEntity Get(Guid id, QueryBuilder<TEntity> queryBuilder = null);
    }
}
