using System;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public class TypeAlreadyRegisteredException : IoContainerException
    {
        public override string Message => $"Type {Type.Name} has already been registered in the container!";

        internal TypeAlreadyRegisteredException(Type type) : base(type) { }
    }
}
