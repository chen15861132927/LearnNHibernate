using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWorkShop.Model.Entities
{
    public class Customer:Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phone { get; set; }
    }
}
