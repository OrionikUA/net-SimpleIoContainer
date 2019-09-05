using System;
using System.Collections.Generic;
using System.Linq;
using OrionikUA.SimpleIoContainer.Exceptions;

namespace OrionikUA.SimpleIoContainer
{
    public class IoContainer
    {
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

        private Creator CreateCreatorByConstructor(Type type)
        {
            return new Creator(() =>
            {
                var constructors = type.GetConstructors();
                return Activator.CreateInstance(type, constructors.First().GetParameters().Select(parameterInfo => GetInstance(parameterInfo.ParameterType)).ToArray());
            });
        }

        private void AddToDictionary(Type type, Creator creator)
        {
            _dictionary.Add(type, creator);
        }

        public T GetInstance<T>()
        {
            return (T) GetInstance(typeof(T));
        }

        public T GetNewInstance<T>()
        {
            var type = typeof(T);
            if (!_dictionary.ContainsKey(type))
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
