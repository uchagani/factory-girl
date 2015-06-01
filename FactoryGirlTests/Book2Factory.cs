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
    public class UserBlah
    {
        public string Name { get; set; }

        public UserBlah()
        {
            Name = "Mike";
        }
    }
    public class Book2Factory
    {
        public void Define()
        {
            FactoryGirl.Define<Book>(() => new
            {
                Author = blah(),
                Title = "The Firm",
                Category = Category.Business,
            }, 
            AfterBuild: book => book.Author = new UserBlah().Name,
            BeforeCreate: book => book.Title = "sdfsadfsdf");
        }

        public string blah()
        {
            var list = new List<int>
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                17,
                18,
                234,
                1812312,
                312,
                8912312,
                138123183,
                1238183,
                183138
            };
            int blah = new Random().Next(list.Count);
            return list[blah].ToString();
        }
    }
}
