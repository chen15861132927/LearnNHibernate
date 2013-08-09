using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWorkShop.Model.Entities;
using MyWorkShop.Model.DTOs;

namespace MyWorkShop.Data.IDAO
{
    public interface IOrderDAO : IDAO<Order, Guid>
    {
        IEnumerable<Order> GetByAmount(decimal minAmount, decimal maxAmount);
        
        IEnumerable<Order> GetByCustomerName(string customerName);
        IEnumerable<Order> GetByCustomerNameViaAlias(string customerName);

        IEnumerable<CustomerIdAndTotalAmountDTO> GetCustomerIdAndTotalAmountDTOs();
        IEnumerable<CustomerNameAndTotalAmountDTO> GetCustomerNameAndTotalAmountDTOs();

        OrderDTO GetOrderDTOById(Guid id);
        IEnumerable<OrderDTO> GetOrderDTOs();

        IEnumerable<OrderDTO> GetOrderDTOsByPage(int pageIndex, int pageSize);
        int GetOrderCount();

        Order GetMaxAmountOrder();

    }
}
