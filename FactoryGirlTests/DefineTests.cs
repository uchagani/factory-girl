using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FactoryGirlTests
{
    [TestClass]
    public class DefineTests : BaseTestMethods
    {


        [TestMethod]
        public void Defining_default_factory_registers_the_factory()
        {
            DefineDefaultUser();
            var factory = FactoryGirl.DefinedFactories.Where(x => x.Item2 == typeof(User));
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Defining_named_factory_registers_the_factory()
        {
            DefineAdminUser();
            var factory = FactoryGirl.DefinedFactories.Where(x => x.Item1 == "AdminUser");
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Defining_multiple_factories_of_they_same_Type_registers_the_factories()
        {
            DefineAdminUser();
            FactoryGirl.Define("HomelessUser", () => new User
            {
                Address = ""
            });
            var count = FactoryGirl.DefinedFactories.Where(x => x.Item2 == typeof(User)).Count();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetDefinedFactories_should_return_all_registered_factories()
        {
            DefineDefaultUser();
            Assert.IsNotNull(FactoryGirl.DefinedFactories.Where(x => x.Item2 == typeof(User)));
            Assert.AreEqual(1, FactoryGirl.DefinedFactories.Count);            
        }

        [TestMethod]
        public void Defining_multiple_factories_of_different_Type_registers_the_factories()
        {
            DefineDefaultUser();
            DefineDefaultBook();
            var userFactory = FactoryGirl.DefinedFactories.Where(x => x.Item2 == typeof(User));              
            var bookFactory = FactoryGirl.DefinedFactories.Where(x => x.Item2 == typeof(Book));
            Assert.IsNotNull(userFactory);         
            Assert.AreEqual(1, userFactory.Count());
            Assert.IsNotNull(bookFactory);
            Assert.AreEqual(1, bookFactory.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateFactoryException))]
        public void Defining_multiple_default_factories_of_same_type_throws_DuplicateFactoryException()
        {
            DefineDefaultBook();
            FactoryGirl.Define(() => new Book
            {
                Author = "Edgar Allen Poe"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateFactoryException))]
        public void Defining_multiple_named_factories_of_the_same_type_throws_DuplicateFactoryException()
        {
            DefineAdminUser();
            FactoryGirl.Define("AdminUser", () => new User
            {
                Name = "Peter Griffin"
            });
        }

        [TestMethod]
        public void ClearDefinitions_should_clear_all_defined_factories()
        {
            DefineDefaultBook();
            DefineDefaultUser();
            DefineAdminUser();
            FactoryGirl.ClearFactoryDefinitions();
            Assert.AreEqual(0, FactoryGirl.DefinedFactories.Count);
        }


    }
}
