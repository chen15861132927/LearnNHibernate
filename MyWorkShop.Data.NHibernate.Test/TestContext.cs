using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate;

namespace MyWorkShop.Data.NHibernate.Test
{
    [SetUpFixture]    
    public class TestContext
    {
        private static Configuration config;
        private static ISessionFactory sessionFactory;

        [SetUp]
        public void InitTestContext()
        {
            System.Console.WriteLine("StartInitTestContext");

            //从Test项目的hibernate.cfg.xml中读取hibernate配置
            config = new Configuration().Configure();
            sessionFactory = config.BuildSessionFactory();

            System.Console.WriteLine("EndInitTestContext");
        }

        //关闭sessionFactory
        [TearDown]
        public void CloseNHibernate()
        {
            System.Console.WriteLine("StartCloseNHibernate");

            sessionFactory.Close();

            System.Console.WriteLine("EndCloseNHibernate");
        }

        public static Configuration Config()
        {
            return config;
        }
    }
}
