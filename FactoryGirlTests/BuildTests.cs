using FactoryGirlCore;
using FactoryGirlTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FactoryGirlTests
{
    [TestClass]
    public class BuildTests : BaseTestMethods
    {
        [TestMethod]
        public void Build_model_with_default_factory_returns_model()
        {
            DefineDefaultUser();
            var user = FactoryGirl.Build<User>();
            Assert.AreEqual("Umair", user.Name);
        }

        [TestMethod]
        public void BuildList_of_model_with_default_factory_returns_list_of_model()
        {
            DefineDefaultUser();
            var users = FactoryGirl.BuildList<User>(5);
            Assert.AreEqual(5, users.Count);
            users.ToList().ForEach(x => Assert.AreEqual("Umair", x.Name));
        }

        [TestMethod]
        public void Build_model_with_default_factory_with_overrides_returns_model_with_overrides()
        {
            DefineDefaultUser();
            string name = "Batman";
            var user = FactoryGirl.Build<User>(u => u.Name = name);
            Assert.AreEqual(name, user.Name);
        }

        [TestMethod]
        public void BuildList_of_model_with_default_factory_with_overrides_returns_list_of_model_with_overrides()
        {
            DefineDefaultUser();
            string name = "Batman";
            var users = FactoryGirl.BuildList<User>(5, u => u.Name = name);
            Assert.AreEqual(5, users.Count);
            users.ToList().ForEach(x => Assert.AreEqual(name, x.Name));
        }

        [TestMethod]
        public void Build_model_with_named_factory_returns_model()
        {
            DefineAdminUser();
            var user = FactoryGirl.Build<User>("AdminUser");
            Assert.AreEqual(Role.Admin, user.Role);
        }

        [TestMethod]
        public void BuildList_of_model_with_named_factory_returns_list_of_model()
        {
            DefineAdminUser();
            var users = FactoryGirl.BuildList<User>(5, "AdminUser");
            Assert.AreEqual(5, users.Count);
            users.ToList().ForEach(x => Assert.AreEqual(Role.Admin, x.Role));
        }

        [TestMethod]
        public void Build_model_with_named_factory_with_overrides_returns_model_with_overrides()
        {
            DefineAdminUser();
            string name = "Batman";
            var user = FactoryGirl.Build<User>("AdminUser", x => x.Name = name);
            Assert.AreEqual(name, user.Name);
        }

        [TestMethod]
        public void BuildList_of_model_with_named_factory_with_overrides_returns_list_of_model_with_overrides()
        {
            DefineAdminUser();
            string name = "Batman";
            var users = FactoryGirl.BuildList<User>(5, "AdminUser", x => x.Name = name);
            Assert.AreEqual(5, users.Count);
            users.ToList().ForEach(x => Assert.AreEqual(name, x.Name));
        }
    }
}
