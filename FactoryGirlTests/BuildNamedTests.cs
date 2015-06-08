using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactoryGirlTests
{
    [TestClass]
    public class BuildNamedTests
    {
        private BookFactory factory;

        [TestInitialize]
        public void Init()
        {
            factory = new BookFactory();
            factory.DefineNamed();
        }

        [TestCleanup]
        public void Clean()
        {
            FactoryGirl.ClearFactoryDefinitions();
        }

        [TestMethod]
        public void build_named_factory_without_callbacks()
        {
            var book = FactoryGirl.Build<Book>("Named", SkipCallbacks: true);
            Assert.AreEqual(typeof(Book), book.GetType());
            Assert.AreEqual("Named Book", book.Title);
            Assert.AreEqual("Named Author", book.Author);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Art, book.Category);
        }

        [TestMethod]
        public void build_named_factory()
        {
            var book = FactoryGirl.Build<Book>("Named");
            Assert.AreEqual(typeof(Book), book.GetType());
            Assert.AreEqual("Named Book", book.Title);
            Assert.AreEqual("Named Author", book.Author);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Travel, book.Category);
        }

        [TestMethod]
        public void build_named_factory_with_overrides_without_callbacks()
        {
            const string author = "Override Author";
            var book = FactoryGirl.Build<Book>("Named", x => x.Author = author, SkipCallbacks: true);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual("Named Book", book.Title);
            Assert.AreEqual(123, book.Isbn);
            Assert.AreEqual(Category.Art, book.Category);
        }
    }
}
