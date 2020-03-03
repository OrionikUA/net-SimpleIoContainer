namespace OrionikUA.SimpleIoContainer.Tests.Helpers
{
    public class SeveralConstructorsWithSeveralPreferredAttributesClass
    {
        [PreferredConstructorForContainer]
        public SeveralConstructorsWithSeveralPreferredAttributesClass(BaseClass baseClass)
        {
            
        }

        [PreferredConstructorForContainer]
        public SeveralConstructorsWithSeveralPreferredAttributesClass(SecondClass secondClass)
        {
            
        }
    }
}