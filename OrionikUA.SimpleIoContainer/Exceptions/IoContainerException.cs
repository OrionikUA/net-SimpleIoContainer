using System;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public abstract class IoContainerException : Exception
    {
        public Type Type { get; }

        protected internal IoContainerException(Type type)
        {
            Type = type;
        }
    }
}
