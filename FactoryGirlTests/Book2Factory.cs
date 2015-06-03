using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FactoryGirlCore;
using FactoryGirlTests.Models;

namespace FactoryGirlTests
{
    public class Book2Factory : IDefinable
    {
        public void Define()
        {
            FactoryGirl.Define(() => new Book
            {
                Author = "Default Author",
                Title = "Default Book",
                Category = Category.Art,
                Isbn = 123
            },
            AfterBuild: book => book.Category = Category.Travel);
        }

        public void DefineNamed()
        {
            FactoryGirl.Define("Named", () => new Book
            {
                Author = "Named Author",
                Title = "Named Book",
                Category = Category.Art,
                Isbn = 123
            },
            AfterBuild: book => book.Category = Category.Travel);
        }
    }
}
