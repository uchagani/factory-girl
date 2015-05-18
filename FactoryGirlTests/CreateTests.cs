using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FactoryGirlTests
{
    [TestClass]
    public class CreateTests : BaseTestMethods
    {
        [TestMethod]
        public void Create_model_with_default_factory_returns_model_calls_Save()
        {
            DefineDefaultBook();
            var book = FactoryGirl.Create<Book>();
            Assert.IsTrue(book.Saved);
        }

        [TestMethod]
        public void CreateList_of_model_with_default_factory_returns_model_calls_Save()
        {
            DefineDefaultBook();
            var books = FactoryGirl.CreateList<Book>(5);
            Assert.AreEqual(5, books.Count);
            books.ToList().ForEach(x => Assert.IsTrue(x.Saved));
        }
        
        [TestMethod]
        public void Create_model_with_default_factory_with_overrides_returns_model_calls_Save()
        {
            DefineDefaultBook();
            var title = "The Firm";
            var book = FactoryGirl.Create<Book>(b => b.Title = title);
            Assert.AreEqual(title, book.Title);
            Assert.IsTrue(book.Saved);
        }

        [TestMethod]
        public void CreateList_of_model_with_default_factory_with_overrides_returns_list_of_model_calls_Save()
        {
            DefineDefaultBook();
            var title = "The Firm";
            var books = FactoryGirl.CreateList<Book>(5, b => b.Title = title);
            Assert.AreEqual(5, books.Count);
            books.ToList().ForEach(x => Assert.IsTrue(x.Saved));
        }

        [TestMethod]
        public void Create_model_with_named_factory_returns_model_calls_Save()
        {
            DefineTravelBook();
            var book = FactoryGirl.Create<Book>("TravelBook");
            Assert.AreEqual(Category.Travel, book.Category);
            Assert.IsTrue(book.Saved);
        }

        [TestMethod]
        public void CreateList_of_model_with_named_factory_returns_list_of_model_calls_Save()
        {
            DefineTravelBook();
            var books = FactoryGirl.CreateList<Book>(5, "TravelBook");
            Assert.AreEqual(5, books.Count);
            books.ToList().ForEach(x => Assert.AreEqual(Category.Travel, x.Category));
            books.ToList().ForEach(x => Assert.IsTrue(x.Saved));
        }

        [TestMethod]
        public void Create_model_with_named_factory_with_overrides_returns_model_with_overrides_calls_Save()
        {
            DefineTravelBook();
            string author = "Rand McNally";
            var book = FactoryGirl.Create<Book>("TravelBook", x => x.Author = author);
            Assert.AreEqual(author, book.Author);
            Assert.IsTrue(book.Saved);
        }

        [TestMethod]
        public void CreateList_of_model_with_named_factory_with_overrides_returns_list_of_model_with_overrides_calls_Save()
        {
            DefineTravelBook();
            string author = "Rand McNally";
            var books = FactoryGirl.CreateList<Book>(5, "TravelBook", x => x.Author = author);
            Assert.AreEqual(5, books.Count);
            books.ToList().ForEach(x => Assert.AreEqual(author, x.Author));
            books.ToList().ForEach(x => Assert.IsTrue(x.Saved));
        }
    }
}
