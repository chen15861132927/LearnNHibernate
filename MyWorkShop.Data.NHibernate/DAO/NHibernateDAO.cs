using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using MyWorkShop.Model.Entities;
using MyWorkShop.Data.IDAO;

namespace MyWorkShop.Data.NHibernate.DAO
{
    public class NHibernateDAO<T,TId> : IDAO<T,TId>
        where T : Entity<TId>
    {

        //使用单例模式创建SessionFactory
        private static ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

        //创建Session
        protected ISession NHibernateSession
        {
            get
            {
                return sessionFactory.OpenSession();
            }
        }


        public T Create(T entity)
        {
            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                session.Save(entity);
                transaction.Commit();
            }
            return entity;
        }

        public void Update(T entity)
        {
            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                session.Update(entity);
                transaction.Commit();
            }
        }

        public void Delete(T entity)
        {
            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(entity);
                transaction.Commit();
            }
        }


        public T GetById(TId id)
        {
            T entity = default(T);

            using (var session = NHibernateSession)
            using (var transaction = session.BeginTransaction())
            {
                entity = session.Get<T>(id); 
                transaction.Commit();
            }
            return entity;
        }
    }
}
