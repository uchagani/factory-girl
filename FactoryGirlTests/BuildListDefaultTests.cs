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
    public class BuildListDefaultTests
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
        public void build_list_default_factory_without_callbacks()
        {
            var books = FactoryGirl.BuildList<Book>(5, SkipCallbacks: true);
            Assert.AreEqual(5, books.Count);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Default Book", book.Title);
                Assert.AreEqual("Default Author", book.Author);
                Assert.AreEqual(123, book.Isbn);
                Assert.AreEqual(Category.Art, book.Category);
            });
        }

        [TestMethod]
        public void build_list_default_factory()
        {
            var books = FactoryGirl.BuildList<Book>(5);
            Assert.AreEqual(5, books.Count);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Default Book", book.Title);
                Assert.AreEqual("Default Author", book.Author);
                Assert.AreEqual(123, book.Isbn);
                Assert.AreEqual(Category.Travel, book.Category);
            });
        }

        [TestMethod]
        public void build_default_factory_list_with_overrides_without_callbacks()
        {
            const string author = "Override Author";
            var books = FactoryGirl.BuildList<Book>(5, x => x.Author = author, SkipCallbacks: true);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Default Book", book.Title);
                Assert.AreEqual(author, book.Author);
                Assert.AreEqual(123, book.Isbn);
                Assert.AreEqual(Category.Art, book.Category);
            });
        }

    }
}
