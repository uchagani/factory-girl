using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlCore
{
    public abstract class FactoryBase<T> : IFactory<T>, ICallback<T>
    {
        public T Instance { get; set; }
        public abstract Func<T> Define();

        public virtual Func<T> AfterBuild()
        {
            return () => default(T);
        }

        public virtual Func<T> BeforeCreate()
        {
            return () => default(T);
        }

        public virtual Func<T> AfterCreate()
        {
            return () => default(T);
        }
    }
}
