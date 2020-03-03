using System;

namespace OrionikUA.SimpleIoContainer
{
    public class TypeCreator
    {
        internal Type First { get; private set; }
        internal Type Second { get; private set; }

        //internal Func<T1> Func { get; }

        //public TypeCreator(Func<T1> func = null)
        //{
        //    Func = func;
        //}

        public TypeCreator Create<T1, T2>() where T1 : T2
        {
            return new TypeCreator
            {
                First = typeof(T1),
                Second = typeof(T2)
            };
        }
    }
}