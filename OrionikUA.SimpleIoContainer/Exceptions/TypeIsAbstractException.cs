using System;
using System.Collections.Generic;
using System.Text;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public class TypeIsAbstractException : IoContainerException
    {
        public override string Message => $"Type {Type.Name} is abstract, it cannot be registered!";

        internal TypeIsAbstractException(Type type) : base(type) { }
    }
}
