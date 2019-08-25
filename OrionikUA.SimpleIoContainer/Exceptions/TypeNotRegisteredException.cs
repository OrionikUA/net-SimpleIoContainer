using System;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public class TypeNotRegisteredException : Exception
    {
        public override string Message => $"Type {Type} was not been registered in the container!";
        public Type Type { get; }

        public TypeNotRegisteredException(Type type)
        {
            Type = type;
        }
    }
}
