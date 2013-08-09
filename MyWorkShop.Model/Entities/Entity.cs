using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWorkShop.Model.Entities
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; protected set; }
    }
}
