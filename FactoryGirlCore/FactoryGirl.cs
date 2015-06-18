using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace FactoryGirlCore
{
    public class FactoryGirl
    {
        private static readonly IDictionary<Tuple<string, Type>, dynamic> factories = new Dictionary<Tuple<string, Type>, dynamic>();
        private const string defaultName = "0b99aa69ee034db3b91d5568d7d91977";

        public static ICollection<Tuple<string, Type>> DefinedFactories
        {
            get { return factories.Keys; }
        }

        public static void Define<T>(Func<T> factory, Func<dynamic, object> AfterBuild = null, Func<dynamic, object> BeforeCreate = null, Func<dynamic, object> AfterCreate = null)
        {
            Define(defaultName, factory, AfterBuild, BeforeCreate, AfterCreate);
        }

        public static void Define<T>(string name, Func<T> factory, Func<dynamic, object> AfterBuild = null, Func<dynamic, object> BeforeCreate = null, Func<dynamic, object> AfterCreate = null)
        {
            if (IsDefined(name, typeof (T)))
                throw new DuplicateFactoryException(string.Format("Factory named {0} of Type {1} is already defined", name, typeof(T)));

            dynamic f = new ExpandoObject();
            f.Factory = factory;

            if(AfterBuild != null)
                f.AfterBuild = AfterBuild;
            if (BeforeCreate != null)
                f.BeforeCreate = BeforeCreate;
            if (AfterCreate != null)
                f.AfterCreate = AfterCreate;

            factories.Add(new Tuple<string, Type>(name, typeof(T)), f);
        }

        public static bool IsDefined(string name, Type factoryType)
        {
            return factories.ContainsKey(new Tuple<string, Type>(name, factoryType));
        }
        
        public static bool IsDefined(Type factoryType)
        {
            return factories.ContainsKey(new Tuple<string, Type>(defaultName, factoryType));
        }

        public static bool Contains(ExpandoObject obj, string key)
        {
            return ((IDictionary<string, Object>)obj).ContainsKey(key);
        }

        private static dynamic GetFactoryDefinition(string name, Type type)
        {
            dynamic factoryDef = null;
            if (factories != null && !factories.TryGetValue(new Tuple<string, Type>(name, type), out factoryDef))
                throw new Exception("Undefined factory");

            if (factoryDef == null)
                throw new ArgumentNullException(string.Format("Unable to retreive factory of name {0} and Type {1}", name, type));

            return factoryDef;
        }

        public static T Build<T>(string name = defaultName, bool SkipCallbacks = false)
        {
            return Build<T>(name, x => { }, SkipCallbacks: SkipCallbacks);
        }

        public static ICollection<T> BuildList<T>(int count, string name = defaultName, bool SkipCallbacks = false)
        {
            return BuildList<T>(count, name, x => { }, SkipCallbacks: SkipCallbacks);
        }

        public static T Build<T>(Action<T> overrides, bool SkipCallbacks = false)
        {
            return Build(defaultName, overrides, SkipCallbacks: SkipCallbacks);
        }

        public static ICollection<T> BuildList<T>(int count, Action<T> overrides, bool SkipCallbacks = false)
        {
            return BuildList(count, defaultName, overrides, SkipCallbacks = SkipCallbacks);
        }

        public static T Build<T>(string name, Action<T> overrides, bool SkipCallbacks = false)
        {
            dynamic factoryDef = GetFactoryDefinition(name, typeof(T));
            var result = (T)factoryDef.Factory();

            if (result == null)
                throw new ArgumentNullException(string.Format("Error retreiving defined factory.  Name: {0} Type: {1}",
                    name, typeof(T)));

            overrides(result);

            if (!SkipCallbacks && Contains(factoryDef, "AfterBuild"))
                factoryDef.AfterBuild(result);

            return result;
        }

        public static ICollection<T> BuildList<T>(int count, string name, Action<T> overrides, bool SkipCallbacks = false)
        {
            ICollection<T> collection = new List<T>();
            for (int i = 0; i < count; i++)
            {
                var obj = Build<T>(name, overrides, SkipCallbacks);
                collection.Add(obj);
            }
            return collection;
        }

        public static T Create<T>(string name = defaultName, bool SkipCallbacks = false) where T : IRepository<T>
        {
            return Create<T>(name, x => { }, SkipCallbacks: SkipCallbacks);
        }

        public static ICollection<T> CreateList<T>(int count, string name = defaultName, bool SkipCallbacks = false) where T : IRepository<T>
        {
            return CreateList<T>(count, name, x => { }, SkipCallbacks: SkipCallbacks);
        }

        public static T Create<T>(Action<T> overrides, bool SkipCallbacks = false) where T : IRepository<T>
        {
            return Create(defaultName, overrides, SkipCallbacks: SkipCallbacks);
        }

        public static ICollection<T> CreateList<T>(int count, Action<T> overrides, bool SkipCallbacks = false) where T : IRepository<T>
        {
            return CreateList(count, defaultName, overrides, SkipCallbacks: SkipCallbacks);
        }

        public static T Create<T>(string name, Action<T> overrides, bool SkipCallbacks = false) where T : IRepository<T>
        {
            var result = Build(name, overrides, SkipCallbacks: SkipCallbacks);

            dynamic factoryDef = GetFactoryDefinition(name, typeof(T));

            if (!SkipCallbacks && Contains(factoryDef, "BeforeCreate"))
                factoryDef.BeforeCreate(result);

            result.Save();

            if (!SkipCallbacks && Contains(factoryDef, "AfterCreate"))
                factoryDef.AfterCreate(result);

            return result;
        }

        public static ICollection<T> CreateList<T>(int count, string name, Action<T> overrides, bool SkipCallbacks = false) where T : IRepository<T>
        {
            var objList = BuildList(count, name, overrides, SkipCallbacks: SkipCallbacks);

            dynamic factoryDef = GetFactoryDefinition(name, typeof(T));

            objList.ToList().ForEach(x =>
            {
                if (!SkipCallbacks && Contains(factoryDef, "BeforeCreate"))
                    factoryDef.BeforeCreate(x);

                x.Save();

                if (!SkipCallbacks && Contains(factoryDef, "AfterCreate"))
                    factoryDef.AfterCreate(x);
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
