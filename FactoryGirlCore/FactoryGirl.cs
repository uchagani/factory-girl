using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FactoryGirlCore
{
    public class FactoryGirl
    {
        private static readonly List<Factory> factories = new List<Factory>();
        private const string defaultName = "0b99aa69ee034db3b91d5568d7d91977";

        public static ICollection<Factory> DefinedFactories
        {
            get { return factories; }
        }

        public static Factory Define<T>(Func<T> factory)
        {
            return Define(defaultName, factory);
        }

        public static Factory Define<T>(string name, Func<T> factory)
        {
            if (IsDefined(name, typeof(T)))
            {
                throw new DuplicateFactoryException(String.Format("A factory named {0} has already been registered for the {1} type.  Only one factory per name per type is allowed.", name, typeof(T)));
            }
            var newFactory = new Factory()
            {
                Name = name,
                Definition = () => factory(),
                Type = typeof(T)
            };
            factories.Add(newFactory);
            return newFactory;
        }
        
        public static bool IsDefined(string name, Type factoryType)
        {
            return factories.Exists(f => f.Name == name && f.Type == factoryType);
        }

        public static T Build<T>(string name = defaultName)
        {
            return Build<T>(name, x => { });
        }

        public static ICollection<T> BuildList<T>(int count, string name = defaultName)
        {
            return BuildList<T>(count, name, x => { });
        }

        public static T Build<T>(Action<T> overrides)
        {
            return Build<T>(defaultName, overrides);
        }

        public static ICollection<T> BuildList<T>(int count, Action<T> overrides)
        {
            return BuildList<T>(count, defaultName, overrides);
        }

        public static T Build<T>(string name, Action<T> overrides)
        {
            var key = new Tuple<string, Type>(name, typeof(T));
            var factory = factories.FirstOrDefault(f => f.Name == name && f.Type == typeof(T));
            if (factory == null) throw new Exception("Factory with name: " + name + " and type: " + typeof(T) + " could not be found");
            factory.ExecuteCallBack(Callback.BeforeBuild);
            var result = (T)factory.Definition();
            overrides(result);
            factory.ExecuteCallBack(Callback.AfterBuild);
            return result;
        }

        public static ICollection<T> BuildList<T>(int count, string name, Action<T> overrides)
        {
            ICollection<T> collection = new List<T>();
            for (int i = 0; i < count; i++)
            {
                var obj = Build<T>(name);
                overrides(obj);
                collection.Add(obj);
            }
            return collection;
        }

        public static T Create<T>(string name = defaultName) where T : IRepository<T>
        {
            return Create<T>(name, x => { });
        }

        public static ICollection<T> CreateList<T>(int count, string name = defaultName) where T : IRepository<T>
        {
            return CreateList<T>(count, name, x => { });
        }

        public static T Create<T>(Action<T> overrides) where T : IRepository<T>
        {
            return Create<T>(defaultName, overrides);
        }

        public static ICollection<T> CreateList<T>(int count, Action<T> overrides) where T : IRepository<T>
        {
            return CreateList<T>(count, defaultName, overrides);
        }

        public static T Create<T>(string name, Action<T> overrides) where T : IRepository<T>
        {
            var factory = factories.FirstOrDefault(f => f.Name == name && f.Type == typeof(T));
            factory.ExecuteCallBack(Callback.BeforeSave);
            var obj = Build<T>(name, overrides);
            obj.Save();
            factory.ExecuteCallBack(Callback.AfterSave);
            return obj;
        }

        public static ICollection<T> CreateList<T>(int count, string name, Action<T> overrides) where T : IRepository<T>
        {
            var objList = BuildList<T>(count, name, overrides);
            var factory = factories.FirstOrDefault(f => f.Name == name && f.Type == typeof(T));
            objList.ToList().ForEach(x =>
            {
                factory.ExecuteCallBack(Callback.BeforeSave);
                x.Save();
                factory.ExecuteCallBack(Callback.AfterSave);
            });
            return objList;
        }

        public static void ClearFactoryDefinitions()
        {
            factories.Clear();
        }

        public static void Initialize(Type type)
        {
            var factoryTypes = type.Assembly.GetTypes().Where(t => typeof(IDefinable).IsAssignableFrom(t));
            foreach (var factoryType in factoryTypes.ToList())
            {
                var factory = (IDefinable)Activator.CreateInstance(factoryType);
                factory.Define();
            }
        }
    }
}
