using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OrionikUA.SimpleIoContainer.Exceptions;

namespace OrionikUA.SimpleIoContainer
{
    public class IoContainer
    {
        private static IoContainer _instance;
        public static IoContainer Default => _instance ?? (_instance = new IoContainer());

        private readonly Dictionary<Type, Creator> _dictionary = new Dictionary<Type, Creator>();

        public void Register<TInterface, T>(Func<T> func) where T : TInterface
        {
            var typeInterface = typeof(TInterface);
            if (_dictionary.ContainsKey(typeInterface))
                throw new TypeAlreadyRegisteredException(typeInterface);
            AddToDictionary(typeInterface, new Creator(() => func()));
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            var typeInterface = typeof(TInterface);
            var typeToCreate = typeof(T);
            if (_dictionary.ContainsKey(typeInterface))
                throw new TypeAlreadyRegisteredException(typeInterface);
            AddToDictionary(typeInterface, CreateCreatorByConstructor(typeToCreate));
        }

        public void Register<T>(Func<T> func)
        {
            var type = typeof(T);
            if (_dictionary.ContainsKey(type))
                throw new TypeAlreadyRegisteredException(type);
            AddToDictionary(type, new Creator(() => func()));
        }

        public void Register<T>()
        {
            var type = typeof(T);
            if (type.IsAbstract)
                throw new TypeIsAbstractException(type);
            if (_dictionary.ContainsKey(type))
                throw new TypeAlreadyRegisteredException(type);
            AddToDictionary(type, CreateCreatorByConstructor(type));
        }

        public void Register<T>(params TypeCreator[] creators)
        {

        }

        private Creator CreateCreatorByConstructor(Type type)
        {
            return new Creator(() =>
            {
                var constructors = type.GetConstructors();
                var constructor = GetConstructor(constructors, type);
                return Activator.CreateInstance(type, constructor.GetParameters().Select(parameterInfo => GetInstance(parameterInfo.ParameterType)).ToArray());
            });
        }

        private ConstructorInfo GetConstructor(ConstructorInfo[] constructors, Type type)
        {
            if (constructors.Length == 1)
                return constructors.First();
            if (constructors.Length > 1)
            {
                var count = 0;
                ConstructorInfo selectedConstructor = null;
                foreach (var constructorInfo in constructors)
                {
                    if (constructorInfo.GetCustomAttributes<PreferredConstructorForContainer>().Any())
                    {
                        selectedConstructor = constructorInfo;
                        count++;
                    }
                }

                if (count == 0)
                    throw new MoreThanOneConstructorException(type, true);
                if (count > 1)
                    throw new MoreThanOneConstructorException(type, false);
                return selectedConstructor;
            }
            throw new ArgumentException("There are no constructors");
        }

        private void AddToDictionary(Type type, Creator creator)
        {
            _dictionary.Add(type, creator);
        }

        public T GetInstance<T>()
        {
            return (T) GetInstance(typeof(T));
        }

        public bool CanGetInstance<T>()
        {
            var type = typeof(T);
            return _dictionary.ContainsKey(type);
        }

        public bool TryGetInstance<T>(out T obj)
        {
            if (CanGetInstance<T>())
            {
                obj = (T)GetInstance(typeof(T));
                return true;
            }
            obj = default;
            return false;
        }

        public T GetNewInstance<T>()
        {
            var type = typeof(T);
            if (!CanGetInstance<T>())
                throw new TypeNotRegisteredException(type);
            return (T) _dictionary[type].NewActivation();
        }

        private object GetInstance(Type type)
        {
            if (!_dictionary.ContainsKey(type))
                throw new TypeNotRegisteredException(type);
            return _dictionary[type].Activation();
        }

    }
}
