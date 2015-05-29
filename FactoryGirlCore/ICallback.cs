using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlCore
{
    public interface ICallback<T>
    {
        Func<T> AfterBuild();
        Func<T> BeforeCreate();
        Func<T> AfterCreate();
    }
}
