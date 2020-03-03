using System;

namespace OrionikUA.SimpleIoContainer.Tests.Helpers
{
    public class SeveralConstructorsWithPreferredAttributeClass
    {
        public Type Type { get; }
        
        public SeveralConstructorsWithPreferredAttributeClass(BaseClass baseClass)
        {
            Type = baseClass.GetType();
        }

        [PreferredConstructorForContainer]
        public SeveralConstructorsWithPreferredAttributeClass(SecondClass secondClass)
        {
            Type = secondClass.GetType();
        }
    }
}