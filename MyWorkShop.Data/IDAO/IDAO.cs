using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWorkShop.Model.Entities;

namespace MyWorkShop.Data.IDAO
{
    public interface IDAO<T,TId> where T:Entity<TId>
    {
        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        T GetById(TId id);
    }
}
