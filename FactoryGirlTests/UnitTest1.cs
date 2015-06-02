using System;
using System.Reflection;
using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactoryGirlTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FactoryGirl.Initialize(Assembly.GetExecutingAssembly());
            var book = FactoryGirl.Build<Book>();
        }

        [TestMethod]
        public void blah()
        {
            Book2Factory factory = new Book2Factory();
            factory.Define();
            var x = FactoryGirl.Build<Book>();
            var y = FactoryGirl.BuildList<Book>(2);
        }
    }
}
