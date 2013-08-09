using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;

namespace MyWorkShop.Data.NHibernate.Test.DAOTests
{
    public abstract class BaseDAOTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            System.Console.WriteLine("StartSetUp");


            //每个测试方法开始运行时，删除已有的数据表，然后创建新的数据表
            new SchemaExport(TestContext.Config()).Execute(true, true, false);

            //初始化测试数据
            SetData();


            System.Console.WriteLine("EndSetUp");
        }

        //初始化测试数据
        //初始数据的方法定义到各个测试子类中——使用“模板方法”设计模式(Template Method)
        protected virtual void SetData()
        {
        }
    }
}
