using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactoryGirlTests
{
    [TestClass]
    public class BaseTestMethods
    {
        [TestInitialize]
        public void Cleanup()
        {
            FactoryGirl.ClearFactoryDefinitions();
        }

        public void DefineDefaultUser()
        {
            FactoryGirl.Define(() => new User
            {
                Name = "Umair",
                Role = Role.User,
                Address = "123 None of your business street."
            });
        }

        public void DefineAdminUser()
        {
            FactoryGirl.Define("AdminUser", () => new User
            {
                Name = "John Smith",
                Role = Role.Admin
            });
        }

        public void DefineDefaultBook()
        {
            FactoryGirl.Define<Book>(() => new Book
            {
                Author = "James Patterson",
                Isbn = 1234567890
            });
        }

        public void DefineTravelBook()
        {
            FactoryGirl.Define<Book>("TravelBook", () => new Book
            {
                Category = Category.Travel
            });
        }
    }
}
