﻿using System;
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
    public class CreateListNamedTests
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
        public void create_list_named_factory_without_callbacks()
        {
            var books = FactoryGirl.CreateList<Book>(5, "Named", SkipCallbacks: true);
            Assert.AreEqual(5, books.Count);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Named Book", book.Title);
                Assert.AreEqual("Named Author", book.Author);
                Assert.AreEqual(123, book.Isbn);
                Assert.AreEqual(Category.Art, book.Category);
                Assert.IsTrue(book.Saved);
            });
        }

        [TestMethod]
        public void create_list_named_factory()
        {
            var books = FactoryGirl.CreateList<Book>(5, "Named");
            Assert.AreEqual(5, books.Count);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Hello, World!", book.Title);
                Assert.AreEqual("Named Author", book.Author);
                Assert.AreEqual(-1, book.Isbn);
                Assert.AreEqual(Category.Travel, book.Category);
                Assert.IsTrue(book.Saved);
            });
        }

        [TestMethod]
        public void create_named_factory_list_with_overrides_without_callbacks()
        {
            const string author = "Override Author";
            var books = FactoryGirl.CreateList<Book>(5, "Named", x => x.Author = author, SkipCallbacks: true);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Named Book", book.Title);
                Assert.AreEqual(author, book.Author);
                Assert.AreEqual(123, book.Isbn);
                Assert.AreEqual(Category.Art, book.Category);
                Assert.IsTrue(book.Saved);
            });
        }

        [TestMethod]
        public void create_named_factory_list_with_overrides()
        {
            const string author = "Override Author";
            var books = FactoryGirl.CreateList<Book>(5, "Named", x => x.Author = author);

            books.ToList().ForEach(book =>
            {
                Assert.AreEqual(typeof(Book), book.GetType());
                Assert.AreEqual("Hello, World!", book.Title);
                Assert.AreEqual(author, book.Author);
                Assert.AreEqual(-1, book.Isbn);
                Assert.AreEqual(Category.Travel, book.Category);
                Assert.IsTrue(book.Saved);
            });
        }

    }
}
