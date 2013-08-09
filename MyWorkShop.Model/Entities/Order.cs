using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWorkShop.Model.Entities
{
    //Guid做主键
    public class Order : Entity<Guid>
    {
        //下单客户
        public virtual Customer Customer { get; set; }
        //下单时间
        public virtual DateTime OrderedDateTime { get; set; }
        //订单金额
        public virtual Decimal? Amount { get; set; } 
    }
}
