using System;
using System.Linq;
using System.Reflection;
using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactoryGirlTests
{
    [TestClass]
    public class BuildDefaultTests
    {
        private Book2Factory factory;

        [TestInitialize]
        public void Init()
        {
            factory = new Book2Factory();
            factory.Define();
        }

        [TestCleanup]
        public void Clean()
        {
            FactoryGirl.ClearFactoryDefinitions();
        }

        [TestMethod]
        public void build_default_factory_without_callbacks()
        {
            var book = FactoryGirl.Build<Book>(SkipCallbacks:true);
            Assert.AreEqual(typeof(Book), book.GetType());
            Assert.AreEqual("Default Book", book.Title);
            Assert.AreEqual("Default Author", book.Author);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Art, book.Category);
        }

        [TestMethod]
        public void build_default_factory()
        {
            var book = FactoryGirl.Build<Book>();
            Assert.AreEqual(typeof(Book), book.GetType());
            Assert.AreEqual("Default Book", book.Title);
            Assert.AreEqual("Default Author", book.Author);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Travel, book.Category);
        }

        [TestMethod]
        public void build_default_factory_with_overrides_without_callbacks()
        {
            const string author = "Override Author";
            var book = FactoryGirl.Build<Book>(x => x.Author = author, SkipCallbacks: true);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual("Default Book", book.Title);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Art, book.Category);
        }
    }
}
