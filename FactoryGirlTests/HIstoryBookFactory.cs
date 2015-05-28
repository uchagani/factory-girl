using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryGirlCore;
using FactoryGirlTests.Models;

namespace FactoryGirlTests
{
    public class HistoryBookFactory : IFactory<Book>, ICallback
    {
        public HistoryBookFactory()
        {
            Instance = new Book();
        }

        public void BeforeBuild()
        {
            FactoryGirl.Create<User>();
        }

        public Book Instance { get; set; }

        public Book Build()
        {
            Define();
            BeforeBuild();
            AfterBuild();
            return Instance;
        }

        public Book Create()
        {
            Build();
            BeforeCreate();
            Instance.Save();
            AfterCreate();
            return Instance;
        }

        public void Define()
        {
            Instance.Author = "John Grisham";
            Instance.Category = Category.History;
            Instance.Borrower = FactoryGirl.Create<User>().Name;
        }

        public void AfterCreate()
        {
            //throw new NotImplementedException();
        }

        public void BeforeCreate()
        {
            throw new NotImplementedException();
        }

        public void AfterBuild()
        {
        }
    }
}
