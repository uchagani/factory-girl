using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGirlCore
{
    public class Factory
    {
        public string Name;
        public Type Type;
        public Func<object> Definition;
        public Dictionary<Callback, Func<object>> Callbacks;
        public Dictionary<object, Type> CallbackObjects;

        public Factory CallBack<T>(Callback callbackType, Func<T> callback)
        {
            Callbacks[callbackType] = () => callback();
            return this;
        }

        public void ExecuteCallBack(Callback callbackType)
        {
            if (Callbacks.ContainsKey(callbackType))
            {
                var result = Callbacks[callbackType]();
                CallbackObjects.Add(result, result.GetType());
            }
        }
    }

    public enum Callback
    {
        BeforeBuild,
        AfterBuild,
        BeforeSave,
        AfterSave
    }
}
