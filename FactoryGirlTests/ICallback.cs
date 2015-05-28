using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlTests
{
    public interface ICallback
    {
        void BeforeBuild();
        void AfterBuild();
        void BeforeCreate();
        void AfterCreate();
    }
}
