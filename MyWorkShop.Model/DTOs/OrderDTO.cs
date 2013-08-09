using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWorkShop.Model.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderedDateTime { get; set; }
        public Decimal? Amount { get; set; } 
    }
}
