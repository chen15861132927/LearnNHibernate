using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using MyWorkShop.Model.Entities;
using MyWorkShop.Data.IDAO;

namespace MyWorkShop.Data.NHibernate.DAO
{
    public class CustomerDAO:NHibernateDAO<Customer,int>, ICustomerDAO
    {
        //筛选条件：根据客户姓名查找客户，假设客户姓名唯一
        public Customer GetByName(string customerName)
        {
            Customer entity = null;
            
            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                //SingleOrDefault:若查询结果不唯一，则抛出异常NHibernate.NonUniqueResultException : query did not return a unique result
                entity = session.QueryOver<Customer>()
                    .Where(c => c.Name == customerName)
                    .SingleOrDefault();

                transaction.Commit();
            }
            return entity;
        }

        //筛选条件：根据客户姓名模糊查找客户
        public IEnumerable<Customer> GetByLikeName(string likeName)
        {
            IEnumerable<Customer> list = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                list = session.QueryOver<Customer>()
                    .WhereRestrictionOn(o => o.Name).IsLike(likeName, MatchMode.Anywhere)
                    .List();

                transaction.Commit();
            }

            return list;
        }

        //筛选条件：根据客户地址查找多个客户
        public IEnumerable<Customer> GetByAddress(string address)
        {
            IEnumerable<Customer> list = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                list = session.QueryOver<Customer>()
                    .Where(c => c.Address == address)
                    .List();

                transaction.Commit();
            }

            return list;
        }
    }
}
