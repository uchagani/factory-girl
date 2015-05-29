using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryGirlCore;
using FactoryGirlTests.Models;

namespace FactoryGirlTests
{
    public class BookFactory : FactoryBase<Book>
    {
        public override Func<Book> Define()
        {
            return () => new Book()
            {
                Category = Category.Business
            };
        }

        public override Func<Book> AfterBuild()
        {
            return () =>
            {
                Instance.Author = "Voldemort";
                return Instance;
            };
        }
        
    }
}
