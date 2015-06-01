using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace FactoryGirlCore
{
    public class FactoryGirl
    {
        private static readonly IDictionary<Tuple<string, Type>, Func<object>> factories = new Dictionary<Tuple<string, Type>, Func<object>>();
        private const string defaultName = "0b99aa69ee034db3b91d5568d7d91977";

        private static readonly IDictionary<Tuple<string, Type>, List<Func<object>>> newFactories = new Dictionary<Tuple<string, Type>, List<Func<object>>>(); 

        public static ICollection<System.Tuple<string, System.Type>> DefinedFactories
        {
            get { return factories.Keys; }
        }

        public static void Define<T>(IFactory<T> factory)
        {
            if (IsDefined(defaultName, typeof(T)))
            {
                throw new DuplicateFactoryException(String.Format("A factory named {0} has already been registered for the {1} type.  Only one factory per name per type is allowed.", defaultName, typeof(T)));
            }

            newFactories.Add(new Tuple<string, Type>(defaultName, typeof(T)), new List<Func<object>>
            {
                factory.Define,
                factory.AfterBuild,
                factory.BeforeCreate,
                factory.AfterCreate
            });
        }

        private static readonly IDictionary<Tuple<string, Type>, Func<object>> blahFactories = new Dictionary<Tuple<string, Type>, Func<object>>();

        private static readonly IDictionary<Tuple<string, Type>, dynamic> finalFactories =
            new Dictionary<Tuple<string, Type>, dynamic>();

        public static void Define<T>(Func<object> factory, Func<dynamic, object> AfterBuild = null, Func<dynamic, object> BeforeCreate = null, Func<dynamic, object> AfterCreate = null)
        {
            //blahFactories.Add(new Tuple<string, Type>(defaultName,typeof(T)), factory);

            dynamic f = new ExpandoObject();
            f.Factory = factory;
            f.AfterBuild = AfterBuild;
            f.BeforeCreate = BeforeCreate;
            finalFactories.Add(new Tuple<string, Type>(defaultName, typeof(T)), f);
        }

        public static T Build<T>()
        {
            //var factoryDef = finalFactories[new Tuple<string, Type>(defaultName, typeof(T))];

            dynamic factoryDef;
            if (!finalFactories.TryGetValue(new Tuple<string, Type>(defaultName, typeof (T)), out factoryDef))
                throw new Exception("Undefined factory");


            var instance = Activator.CreateInstance(typeof(T));
            var instanceProperties = instance.GetType().GetProperties();

            var factory = factoryDef.Factory();
            var factoryType = factory.GetType();

            foreach (var property in instanceProperties)
            {
                var propName = property.Name;
                try
                {
                    var propValue = factoryType.GetProperty(propName).GetValue(factory, null);
                    property.SetValue(instance, propValue, null);
                }
                catch (NullReferenceException nref)
                {
                    //Swallow exception since it was most likely thrown because a property was not set in the factory definition
                }
                catch (RuntimeBinderException rbex)
                {
                    //Swallow exception since it was most likely thrown because a property was not set in the factory definition
                }
            }

            factoryDef.AfterBuild(instance);
            return (T)instance;
        }

        public static void Define<T>(Func<T> factory)
        {
            Define(defaultName, factory);
        }

        public static void Define<T>(string name, Func<T> factory)
        {
            if (IsDefined(name, typeof(T)))
            {
                throw new DuplicateFactoryException(String.Format("A factory named {0} has already been registered for the {1} type.  Only one factory per name per type is allowed.", name, typeof(T)));
            }

            factories.Add(new Tuple<string, Type>(name, typeof(T)), () => factory());
        }

        public static bool IsDefined(string name, Type factoryType)
        {
            return factories.ContainsKey(new Tuple<string, Type>(name, factoryType));
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
            var result = (T)factories[key]();
            overrides(result);
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
            var obj = Build<T>(name, overrides);
            obj.Save();
            return obj;
        }

        public static ICollection<T> CreateList<T>(int count, string name, Action<T> overrides) where T : IRepository<T>
        {
            var objList = BuildList<T>(count, name, overrides);
            objList.ToList().ForEach(x => x.Save());
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

        public static void Initialize(Assembly assembly)
        {
            var factoryTypes = assembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType 
                    && t.BaseType.GetGenericTypeDefinition() == typeof(FactoryBase<>));
            
            foreach (var f in factoryTypes.ToList())
            {
                dynamic factory = Activator.CreateInstance(f);
                Define(factory);
                //factory.Initialize();
            }
        }
    }
}
