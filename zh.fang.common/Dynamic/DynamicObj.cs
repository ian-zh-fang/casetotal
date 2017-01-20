namespace zh.fang.common.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    public class DynamicObj : DynamicObject, IDisposable
    {
        private readonly Dictionary<string, object> _dataContainer = new Dictionary<string, object>();

        public object this[string name]
        {
            get
            {
                object value = null;
                _dataContainer.TryGetValue(name, out value);
                return value;
            }
            set
            {
                var exist = _dataContainer.ContainsKey(name);
                if (exist)
                {
                    _dataContainer[name] = value;
                }

                if (!exist)
                {
                    _dataContainer.Add(name, value);
                }
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        private bool _isDisposed = false;

        protected virtual void Disposed(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {

            }

            _dataContainer.Clear();
            _isDisposed = true;
        }

        void IDisposable.Dispose()
        {
            Disposed(true);
            GC.SuppressFinalize(this);
        }

        ~DynamicObj()
        {
            Disposed(true);
        }
    }
}
