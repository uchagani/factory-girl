using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactoryGirlCore
{
    public interface IRepository<T>
    {
        T Save();
    }
}
