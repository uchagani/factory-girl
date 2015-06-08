using System;
using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FactoryGirlTests
{
    [TestClass]
    public class DefineTests
    {
        private BookFactory bookFactory;

        [TestInitialize]
        public void Init()
        {
            bookFactory = new BookFactory();
            bookFactory.Define();
        }

        [TestCleanup]
        public void Clean()
        {
            FactoryGirl.ClearFactoryDefinitions();
        }

        [TestMethod]
        public void defining_default_factory_registers_the_factory()
        {
            var factory = FactoryGirl.DefinedFactories.FirstOrDefault(x => x.Item2 == typeof(Book));
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void defining_named_factory_registers_the_factory()
        {
            var factory = FactoryGirl.DefinedFactories.Where(x => x.Item1 == "AdminUser");
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void defining_multiple_factories_of_they_same_Type_registers_the_factories()
        {
            FactoryGirl.Define("Fiction Book", () => new Book
            {
                Author = "James Patterson"
            });
            var count = FactoryGirl.DefinedFactories.Count(x => x.Item2 == typeof(Book));
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void defined_factories_should_return_all_registered_factories()
        {
            var factories = FactoryGirl.DefinedFactories;
            Assert.IsNotNull(factories);
            Assert.AreEqual(1, factories.Count);            
        }

        [TestMethod]
        public void defining_multiple_factories_of_different_Type_registers_the_factories()
        {
            FactoryGirl.Define(() => new User
            {
                Address = "123 Nothing Street"
            });

            var factories = FactoryGirl.DefinedFactories;

            var book = factories.Where(b => b.Item2 == typeof(Book));
            var user = factories.Where(u => u.Item2 == typeof (User));
            
            Assert.AreEqual(1, book.Count());
            Assert.AreEqual(1, user.Count());
            Assert.IsNotNull(book.FirstOrDefault());
            Assert.IsNotNull(user.FirstOrDefault());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateFactoryException))]
        public void defining_multiple_default_factories_of_same_type_throws_DuplicateFactoryException()
        {
            FactoryGirl.Define(() => new Book
            {
                Author = "Edgar Allen Poe"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateFactoryException))]
        public void defining_multiple_named_factories_of_the_same_type_throws_DuplicateFactoryException()
        {
            FactoryGirl.Define("AdminUser", () => new User
            {
                Name = "Peter Griffin"
            });

            FactoryGirl.Define("AdminUser", () => new User
            {
                Role = Role.User
            });
        }

        [TestMethod]
        public void clear_definitions_should_clear_all_defined_factories()
        {
            FactoryGirl.ClearFactoryDefinitions();
            Assert.AreEqual(0, FactoryGirl.DefinedFactories.Count);
        }
    }
}
