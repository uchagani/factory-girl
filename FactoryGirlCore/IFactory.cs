using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlCore
{
    public interface IFactory<T> //: IDefinable
    {
        T Instance { get; set; }
        Func<T> Define();
        Func<T> AfterBuild();
        Func<T> BeforeCreate();
        Func<T> AfterCreate();
    }
}
