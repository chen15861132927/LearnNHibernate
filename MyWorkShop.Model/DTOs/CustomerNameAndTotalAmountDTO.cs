using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWorkShop.Model.DTOs
{
    public class CustomerNameAndTotalAmountDTO
    {
        //客户姓名
        public string CustomerName { get; set; }
        //该客户所有订单的总金额
        public Decimal TotalAmount { get; set; }
    }
}
