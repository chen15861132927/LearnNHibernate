using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWorkShop.Data.IDAO;
using MyWorkShop.Model.Entities;
using MyWorkShop.Model.DTOs;
using NHibernate.Transform;
//NHibernate.Criterion.Order与Order实体类类名重复，所以需用别名
using NHibernateCriterion=NHibernate.Criterion;

namespace MyWorkShop.Data.NHibernate.DAO
{
    public class OrderDAO : NHibernateDAO<Order,Guid>,IOrderDAO
    {

        #region 筛选条件
        //筛选条件：查找金额在指定范围内的订单
        public IEnumerable<Order> GetByAmount(decimal minAmount, decimal maxAmount)
        {
            IEnumerable<Order> list = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                //简捷写法：一个where方法包含多个条件，条件之间用&&
                //list = session.QueryOver<Order>()
                //    .Where(o => o.Amount >= minAmount && o.Amount <= maxAmount)
                //    .List();

                list = session.QueryOver<Order>()
                    .Where(o => o.Amount >= minAmount)
                    .And(o => o.Amount <= maxAmount)
                    .OrderBy(o => o.Amount).Desc
                    .List();

                //between...and...写法
                //list = session.QueryOver<Order>()
                //    .WhereRestrictionOn(o=>o.Amount)
                //    .IsBetween(minAmount).And(maxAmount)
                //    .OrderBy(o=>o.Amount).Desc
                //    .List();

                transaction.Commit();
            }

            return list;
        }

        #endregion



        #region 内连接

        //内连接：根据客户姓名查找订单
        public IEnumerable<Order> GetByCustomerName(string customerName)
        {
            IEnumerable<Order> list = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                list = session.QueryOver<Order>()
                    .OrderBy(o=>o.Amount).Desc
                    .Inner.JoinQueryOver<Customer>(o => o.Customer)
                    .Where(c => c.Name == customerName)
                    .List();

                transaction.Commit();
            }

            return list;
        }

        //使用别名进行内连接：根据客户姓名查找订单
        public IEnumerable<Order> GetByCustomerNameViaAlias(string customerName)
        {
            //定义用于内连接的别名变量，该变量必须赋值为null
            Customer customer = null;

            IEnumerable<Order> list = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                list = session.QueryOver<Order>()
                    .JoinAlias(o => o.Customer, () => customer) //指定别名customer
                    .Where(() => customer.Name == customerName)
                    .List();

                transaction.Commit();
            }

            return list;
        }

        #endregion



        #region 投影

        //投影且把投影结果转成DTO：根据订单号查找订单并返回订单OrderDTO
        public OrderDTO GetOrderDTOById(Guid id)
        {
            OrderDTO dto = null;

            //定义用于内连接的别名变量，该变量必须赋值为null
            Customer customer = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                dto = session.QueryOver<Order>()
                    //创建用于内连接的别名customer
                    .JoinAlias(o => o.Customer, () => customer)
                    .Where(o => o.Id == id)
                    .SelectList(list =>list
                        .Select(o => o.Id).WithAlias(() => dto.Id)    //给投影列取别名，用于把投影结果转成DTO
                        .Select(o => customer.Name).WithAlias(() => dto.CustomerName)
                        .Select(o => o.OrderedDateTime).WithAlias(() => dto.OrderedDateTime)
                        .Select(o => o.Amount).WithAlias(() => dto.Amount)
                    )
                    //把投影结果转成DTO
                    .TransformUsing(Transformers.AliasToBean<OrderDTO>())
                   .SingleOrDefault<OrderDTO>();

                transaction.Commit();
            }

            return dto;
        }
        /*  这种写法也行，使用LINQ to Object
        public OrderDTO GetOrderDTOById(Guid id)
        {
            OrderDTO dto = null;

            Customer customer = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                dto = session.QueryOver<Order>()
                    .JoinAlias(o => o.Customer, () => customer)
                    .Where(o => o.Id == id)
                    .Select(o => o.Id, o => customer.Name, o => o.OrderedDateTime, o => o.Amount)
                    //这里只能用List不能用SingleOrDefault，否则props[0]不能使用数组
                    .List<object[]>()
                   .Select(props => new OrderDTO
                    {
                        Id = (Guid)props[0],
                        CustomerName=(string)props[1],
                        OrderedDateTime = (DateTime)props[2],
                        Amount = (decimal)props[3]
                    }).ElementAt(0);

                transaction.Commit();
            }

            return dto;
        }
        */
        
        
        public IEnumerable<OrderDTO> GetOrderDTOs()
        {
            OrderDTO dto = null;
            Customer customer = null;

            IEnumerable<OrderDTO> retList = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                
                retList = session.QueryOver<Order>()
                    .JoinAlias(o => o.Customer, () => customer)
                    .SelectList(list => list
                        .Select(o => o.Id).WithAlias(() => dto.Id)
                        .Select(o => customer.Name).WithAlias(() => dto.CustomerName)
                        .Select(o => o.OrderedDateTime).WithAlias(() => dto.OrderedDateTime)
                        .Select(o => o.Amount).WithAlias(() => dto.Amount)
                    )
                    .OrderBy(o=>o.Amount).Asc
                    .TransformUsing(Transformers.AliasToBean<OrderDTO>())
                   .List<OrderDTO>();
                
                transaction.Commit();
            }

            return retList;
        }

        #endregion




        #region 分组统计

        //分组统计：统计每个客户所有订单的总金额,以及客户Id
        public IEnumerable<CustomerIdAndTotalAmountDTO> GetCustomerIdAndTotalAmountDTOs()
        {
            CustomerIdAndTotalAmountDTO dto = null;

            IEnumerable<CustomerIdAndTotalAmountDTO> retList = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                retList = session.QueryOver<Order>()
                    .SelectList(list => list
                        .SelectGroup(o => o.Customer.Id).WithAlias(() => dto.CustomerId)
                        .SelectSum(o => o.Amount).WithAlias(() => dto.TotalAmount)
                    )
                    .TransformUsing(Transformers.AliasToBean<CustomerIdAndTotalAmountDTO>())
                    .List<CustomerIdAndTotalAmountDTO>();

                transaction.Commit();
            }

            return retList;
        }

        //分组统计：统计每个客户所有订单的总金额，以及客户姓名
        public IEnumerable<CustomerNameAndTotalAmountDTO> GetCustomerNameAndTotalAmountDTOs()
        {
            CustomerNameAndTotalAmountDTO dto = null;
            Customer customer = null;

            IEnumerable<CustomerNameAndTotalAmountDTO> retList = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                retList = session.QueryOver<Order>()
                    .JoinAlias(o => o.Customer, () => customer)
                    .SelectList(list => list
                        .SelectGroup(o => customer.Name).WithAlias(() => dto.CustomerName)
                        .SelectSum(o => o.Amount).WithAlias(() => dto.TotalAmount)
                    )
                    .OrderBy(o => customer.Name).Desc
                    .OrderByAlias(() => dto.TotalAmount).Asc
                    .TransformUsing(Transformers.AliasToBean<CustomerNameAndTotalAmountDTO>())
                    .List<CustomerNameAndTotalAmountDTO>();

                transaction.Commit();
            }

            return retList;
        }

        #endregion




        #region 分页

        //分页
        public IEnumerable<OrderDTO> GetOrderDTOsByPage(int pageIndex, int pageSize)
        {
            OrderDTO dto = null;
            Customer customer = null;

            IEnumerable<OrderDTO> retList = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {

                retList = session.QueryOver<Order>()
                    .JoinAlias(o => o.Customer, () => customer)
                    .SelectList(list => list
                        .Select(o => o.Id).WithAlias(() => dto.Id)
                        .Select(o => customer.Name).WithAlias(() => dto.CustomerName)
                        .Select(o => o.OrderedDateTime).WithAlias(() => dto.OrderedDateTime)
                        .Select(o => o.Amount).WithAlias(() => dto.Amount)
                    )
                    .TransformUsing(Transformers.AliasToBean<OrderDTO>())
                    .OrderBy(o=>o.Amount).Desc
                    .Skip(pageIndex * pageSize).Take(pageSize)
                   .List<OrderDTO>();

                transaction.Commit();
            }

            return retList;
        }
        //订单总数量，用于分页
        public int GetOrderCount()
        {
            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                int count = session.QueryOver<Order>()
                    .RowCount();

                transaction.Commit();

                return count;
            }
        }

        #endregion





        #region 子查询

        //子查询：查找金额最大的订单
        public Order GetMaxAmountOrder()
        {
            Order order = null;

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                //NHibernate.Criterion.Order与Order实体类名重复，所以使用前面定义的别名NHibernateCriterion
                var maxAmount = NHibernateCriterion.QueryOver.Of<Order>()
                    .SelectList(a=>a.SelectMax(o=>o.Amount));
                
                order = session.QueryOver<Order>()
                    .WithSubquery.WhereProperty(o => o.Amount).Eq(maxAmount)
                    .SingleOrDefault();

                transaction.Commit();
            }
            return order;
        }

        #endregion


    }
}
