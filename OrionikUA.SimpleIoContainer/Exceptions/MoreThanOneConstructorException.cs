using System;

namespace OrionikUA.SimpleIoContainer.Exceptions
{
    public class MoreThanOneConstructorException : IoContainerException
    {
        private readonly bool _noAttribute;

        public override string Message =>
            _noAttribute ?
            $"{Type} has too many constructors, make only one constructor, or define PreferredConstructorForContainer attribute for preferred constructor." :
            $"Several constructors of type {Type} has Attribute PreferredConstructorForContainer, class can have only one constructor with PreferredConstructorForContainer attribute";

        internal MoreThanOneConstructorException(Type type, bool noAttribute) : base(type)
        {
            _noAttribute = noAttribute;
        }
    }
}