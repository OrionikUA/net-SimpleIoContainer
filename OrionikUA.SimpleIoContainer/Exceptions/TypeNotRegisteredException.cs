using System;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public class TypeNotRegisteredException : IoContainerException
    {
        public override string Message => $"Type {Type.Name} was not been registered in the container!";

        internal TypeNotRegisteredException(Type type) : base(type) { }
    }
}
