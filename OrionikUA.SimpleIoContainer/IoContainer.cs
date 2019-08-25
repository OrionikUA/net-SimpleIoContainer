using System;
using System.Collections.Generic;
using System.Linq;
using OrionikUA.SimpleIoContainer.Exceptions;

namespace OrionikUA.SimpleIoContainer
{
    public class IoContainer
    {
        private readonly Dictionary<Type, Creator> _dictionary = new Dictionary<Type, Creator>();

        public void Register<T>()
        {
            var type = typeof(T);
            var creator = new Creator(() =>
            {
                var constructors = type.GetConstructors();
                return Activator.CreateInstance(type, constructors.First().GetParameters().Select(parameterInfo => GetInstance(parameterInfo.ParameterType)).ToArray());
            });
            if (_dictionary.ContainsKey(type))
                _dictionary[type] = creator;
            else
                _dictionary.Add(type, creator);
        }

        public T GetInstance<T>()
        {
            return (T) GetInstance(typeof(T));
        }

        private object GetInstance(Type type)
        {
            if (!_dictionary.ContainsKey(type))
                throw new TypeNotRegisteredException(type);
            return _dictionary[type].Activation();
        }
    }
}
