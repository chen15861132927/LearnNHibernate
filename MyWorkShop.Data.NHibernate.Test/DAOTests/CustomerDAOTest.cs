using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MyWorkShop.Data.IDAO;
using MyWorkShop.Data.NHibernate.DAO;
using MyWorkShop.Model.Entities;

namespace MyWorkShop.Data.NHibernate.Test.DAOTests
{
    [TestFixture]
    public class CustomerDAOTest:BaseDAOTest
    {
        public ICustomerDAO CustomerDAO
        {
            get { return new CustomerDAO(); }
        }

        //初始化测试数据
        Customer oneCustomer;
        Customer twoCustomer;
        Customer threeCustomer;
        protected override void SetData()
        {
            oneCustomer = new Customer
            {
                Name = "Name",
                Address = "Address",
                Phone = "Phone"
            };

            twoCustomer = new Customer
            {
                Name = "Name2",
                Address = "Address2",
                Phone = "Phone2"
            };

            threeCustomer = new Customer
            {
                Name = "Name3",
                //故意与oneCustomer.Address重复
                Address = "Address",
                Phone = "Phone3"
            };
        }


        [Test]
        public void Create()
        {
            //创建客户
            CustomerDAO.Create(oneCustomer);
            
            //验证
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(oneCustomer, fromDb);
            Assert.AreEqual(oneCustomer.Name, fromDb.Name);
            Assert.AreEqual(oneCustomer.Address, fromDb.Address);
            Assert.AreEqual(oneCustomer.Phone, fromDb.Phone);
        }


        [Test]
        public void Update()
        {
            //创建客户
            CustomerDAO.Create(oneCustomer);
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);

            //修改客户信息
            oneCustomer.Name = twoCustomer.Name;
            oneCustomer.Address = twoCustomer.Address;
            oneCustomer.Phone = twoCustomer.Phone;

            //更新到数据库
            CustomerDAO.Update(oneCustomer);
            
            //验证
            fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(twoCustomer.Name, fromDb.Name);
            Assert.AreEqual(twoCustomer.Address, fromDb.Address);
            Assert.AreEqual(twoCustomer.Phone, fromDb.Phone);
        }


        [Test]
        public void Delete()
        {
            CustomerDAO.Create(oneCustomer);
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);

            CustomerDAO.Delete(oneCustomer);

            fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNull(fromDb);
        }
        
        [Test]
        public void GetByName()
        {
            CustomerDAO.Create(oneCustomer);
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);

            CustomerDAO.Create(twoCustomer);
            fromDb = CustomerDAO.GetById(twoCustomer.Id);
            Assert.IsNotNull(fromDb);


            fromDb = CustomerDAO.GetByName(oneCustomer.Name);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(oneCustomer.Id, fromDb.Id);
            Assert.AreEqual(oneCustomer.Name, fromDb.Name);
            Assert.AreEqual(oneCustomer.Address, fromDb.Address);
            Assert.AreEqual(oneCustomer.Phone, fromDb.Phone);
        }


        [Test]
        public void GetByLikeName()
        {
            CustomerDAO.Create(oneCustomer);
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);

            CustomerDAO.Create(twoCustomer);
            fromDb = CustomerDAO.GetById(twoCustomer.Id);
            Assert.IsNotNull(fromDb);

            var list = CustomerDAO.GetByLikeName("e");
            Assert.AreEqual(2, list.Count());
            
        }
        


        [Test]
        public void GetByAddress()
        {
            CustomerDAO.Create(oneCustomer);
            var fromDb = CustomerDAO.GetById(oneCustomer.Id);
            Assert.IsNotNull(fromDb);

            CustomerDAO.Create(twoCustomer);
            fromDb = CustomerDAO.GetById(twoCustomer.Id);
            Assert.IsNotNull(fromDb);

            CustomerDAO.Create(threeCustomer);
            fromDb = CustomerDAO.GetById(threeCustomer.Id);
            Assert.IsNotNull(fromDb);


            var list = CustomerDAO.GetByAddress(oneCustomer.Address);
            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(oneCustomer.Id, list.ElementAt(0).Id);
            Assert.AreEqual(threeCustomer.Id, list.ElementAt(1).Id);
        }
    }
}
