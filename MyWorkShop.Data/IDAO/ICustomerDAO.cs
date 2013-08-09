using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWorkShop.Model.Entities;

namespace MyWorkShop.Data.IDAO
{
    public interface ICustomerDAO:IDAO<Customer, int>
    {
        Customer GetByName(string customerName);

        IEnumerable<Customer> GetByLikeName(string likeName);

        IEnumerable<Customer> GetByAddress(string address);
    }
}
