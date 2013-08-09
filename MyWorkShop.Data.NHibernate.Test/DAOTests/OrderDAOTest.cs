using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWorkShop.Data.IDAO;
using MyWorkShop.Data.NHibernate.DAO;
using MyWorkShop.Model.Entities;
using NUnit.Framework;

namespace MyWorkShop.Data.NHibernate.Test.DAOTests
{
    [TestFixture]
    public class OrderDAOTest:BaseDAOTest
    {
        public ICustomerDAO CustomerDAO
        {
            get { return new CustomerDAO(); }
        }
        public IOrderDAO OrderDAO
        {
            get { return new OrderDAO(); }
        }


        //初始化测试数据
        Customer oneCustomer;
        Customer twoCustomer;

        Order oneOrder;
        Order twoOrder;
        Order threeOrder;
        Order fourOrder;

        protected override void SetData()
        {
            oneCustomer = new Customer
            {
                Name = "Name",
                Address = "Address",
                Phone = "Phone"
            };
            CustomerDAO.Create(oneCustomer);


            twoCustomer = new Customer
            {
                Name = "Name2",
                Address = "Address2",
                Phone = "Phone2"
            };
            CustomerDAO.Create(twoCustomer);


            oneOrder = new Order
            {
                Customer = oneCustomer,
                OrderedDateTime = Convert.ToDateTime("2010-1-1"),
                Amount = 100
            };
            twoOrder = new Order
            {
                Customer = oneCustomer,
                OrderedDateTime = Convert.ToDateTime("2010-1-2"),
                Amount = 200
            };
            threeOrder = new Order
            {
                Customer = oneCustomer,
                OrderedDateTime = Convert.ToDateTime("2010-1-3"),
                Amount = 300
            };

            fourOrder = new Order
            {
                Customer = twoCustomer,
                OrderedDateTime = Convert.ToDateTime("2010-1-4"),
                Amount = 400
            };
        }



        [Test]
        public void Create()
        {
            OrderDAO.Create(oneOrder);

            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(oneOrder, fromDb);
            Assert.AreEqual(oneOrder.Customer.Id, fromDb.Customer.Id);
            Assert.AreEqual(oneOrder.OrderedDateTime, fromDb.OrderedDateTime);
            Assert.AreEqual(oneOrder.Amount, fromDb.Amount);
        }


        [Test]
        public void GetByAmount()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);


            var list = OrderDAO.GetByAmount(100,200);
            Assert.AreEqual(2, list.Count());

            //验证排序
            Assert.AreEqual(twoOrder.Id, list.ElementAt(0).Id);
            Assert.AreEqual(oneOrder.Id, list.ElementAt(1).Id);
        }


        [Test]
        public void GetByCustomerName()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(fourOrder);
            fromDb = OrderDAO.GetById(fourOrder.Id);
            Assert.IsNotNull(fromDb);


            var list = OrderDAO.GetByCustomerName(oneCustomer.Name);
            Assert.AreEqual(3, list.Count());

            //排序
            Assert.AreEqual(threeOrder.Id, list.ElementAt(0).Id);
            Assert.AreEqual(twoOrder.Id, list.ElementAt(1).Id);
            Assert.AreEqual(oneOrder.Id, list.ElementAt(2).Id);
        }

        [Test]
        public void GetByCustomerNameViaAlias()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(fourOrder);
            fromDb = OrderDAO.GetById(fourOrder.Id);
            Assert.IsNotNull(fromDb);


            var list = OrderDAO.GetByCustomerNameViaAlias(oneCustomer.Name);
            Assert.AreEqual(3, list.Count());

        }


        [Test]
        public void GetCustomerIdAndTotalAmountDTOs()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(fourOrder);
            fromDb = OrderDAO.GetById(fourOrder.Id);
            Assert.IsNotNull(fromDb);


            var dto = OrderDAO.GetCustomerIdAndTotalAmountDTOs();
            Assert.AreEqual(2, dto.Count());
        }


        [Test]
        public void GetCustomerNameAndTotalAmountDTOs()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(fourOrder);
            fromDb = OrderDAO.GetById(fourOrder.Id);
            Assert.IsNotNull(fromDb);

            var dto = OrderDAO.GetCustomerNameAndTotalAmountDTOs();
            Assert.AreEqual(2, dto.Count());

            Assert.AreEqual(twoCustomer.Name, dto.ElementAt(0).CustomerName);
            Assert.AreEqual(400, dto.ElementAt(0).TotalAmount);

            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(1).CustomerName);
            Assert.AreEqual(600, dto.ElementAt(1).TotalAmount);
        }



        [Test]
        public void GetOrderDTOById()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            var dto = OrderDAO.GetOrderDTOById(oneOrder.Id);
            Assert.AreEqual(oneOrder.Id, dto.Id);
            Assert.AreEqual(oneCustomer.Name, dto.CustomerName);
            Assert.AreEqual(oneOrder.OrderedDateTime, dto.OrderedDateTime);
            Assert.AreEqual(oneOrder.Amount, dto.Amount);
        }

        [Test]
        public void GetOrderDTOs()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            var dto = OrderDAO.GetOrderDTOs();
            Assert.AreEqual(3, dto.Count());

            Assert.AreEqual(oneOrder.Id, dto.ElementAt(0).Id);
            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(0).CustomerName);
            Assert.AreEqual(oneOrder.OrderedDateTime, dto.ElementAt(0).OrderedDateTime);
            Assert.AreEqual(oneOrder.Amount, dto.ElementAt(0).Amount);

            Assert.AreEqual(twoOrder.Id, dto.ElementAt(1).Id);
            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(1).CustomerName);
            Assert.AreEqual(twoOrder.OrderedDateTime, dto.ElementAt(1).OrderedDateTime);
            Assert.AreEqual(twoOrder.Amount, dto.ElementAt(1).Amount);

            Assert.AreEqual(threeOrder.Id, dto.ElementAt(2).Id);
            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(2).CustomerName);
            Assert.AreEqual(threeOrder.OrderedDateTime, dto.ElementAt(2).OrderedDateTime);
            Assert.AreEqual(threeOrder.Amount, dto.ElementAt(2).Amount);
        }

        //分页
        //NHibernate: SELECT TOP (@p0) y0_, y1_, y2_, y3_ FROM (SELECT this_.Id as y0_, customer1_.Name as y1_, this_.OrderedDateTime as y2_, this_.Amount as y3_, ROW_NUMBER() OVER(ORDER BY this_.Amount DESC) as __hibernate_sort_row FROM MyWorkShop_Order this_ inner join MyWorkShop_Customer customer1_ on this_.CustomerId=customer1_.Id) as query WHERE query.__hibernate_sort_row > @p1 ORDER BY query.__hibernate_sort_row;@p0 = 2 [Type: Int32 (0)], @p1 = 2 [Type: Int32 (0)]
        [Test]
        public void GetOrderDTOsByPage()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(fourOrder);
            fromDb = OrderDAO.GetById(fourOrder.Id);
            Assert.IsNotNull(fromDb);

            var dto = OrderDAO.GetOrderDTOsByPage(1,2);
            Assert.AreEqual(2, dto.Count());

            Assert.AreEqual(twoOrder.Id, dto.ElementAt(0).Id);
            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(0).CustomerName);
            Assert.AreEqual(twoOrder.OrderedDateTime, dto.ElementAt(0).OrderedDateTime);
            Assert.AreEqual(twoOrder.Amount, dto.ElementAt(0).Amount);

            Assert.AreEqual(oneOrder.Id, dto.ElementAt(1).Id);
            Assert.AreEqual(oneCustomer.Name, dto.ElementAt(1).CustomerName);
            Assert.AreEqual(oneOrder.OrderedDateTime, dto.ElementAt(1).OrderedDateTime);
            Assert.AreEqual(oneOrder.Amount, dto.ElementAt(1).Amount);

        }

        [Test]
        public void GetOrderCount()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);


            var count = OrderDAO.GetOrderCount();
            Assert.AreEqual(3, count);
        }



        [Test]
        public void GetMaxAmountOrder()
        {
            OrderDAO.Create(oneOrder);
            var fromDb = OrderDAO.GetById(oneOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(twoOrder);
            fromDb = OrderDAO.GetById(twoOrder.Id);
            Assert.IsNotNull(fromDb);

            OrderDAO.Create(threeOrder);
            fromDb = OrderDAO.GetById(threeOrder.Id);
            Assert.IsNotNull(fromDb);


            var order = OrderDAO.GetMaxAmountOrder();
            Assert.IsNotNull(order);
            Assert.AreEqual(order.Id, threeOrder.Id);
        }
    }
}
