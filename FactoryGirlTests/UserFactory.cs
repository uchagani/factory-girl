using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryGirlTests.Models;

namespace FactoryGirlTests
{
    public class UserFactory : IFactory<User>
    {
        public User Instance { get; set; }

        public User Build()
        {
            Define();
            return Instance;
        }

        public User Create()
        {
            Build();
            Instance.Save();
            return Instance;
        }

        public void Define()
        {
            Instance = new User()
            {
                Role = Role.Admin,
                Name = "Chaggs"
            };
        }
    }
}
