using System;

namespace OrionikUA.SimpleIoContainer
{
    internal class Creator
    {
        private object _item;
        private readonly Func<object> _func;

        public Creator(Func<object> func)
        {
            _func = func;
        }

        public object Activation() => _item ?? (_item = NewActivation());

        public object NewActivation() => _func?.Invoke();
    }
}
