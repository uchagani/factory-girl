using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlTests
{
    public interface IFactory<T>
    {
        T Instance { get; set; }
        T Build();
        T Create();
        void Define();
    }
}
