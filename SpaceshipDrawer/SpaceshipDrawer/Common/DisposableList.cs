using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceshipDrawer.Common
{
    class DisposableList : IDisposable
    {
        Object _lock = new Object();
        Stack<IDisposable> _objs = new Stack<IDisposable>();

        public DisposableList()
        {
        }

        public void Add(IDisposable obj)
        {
            lock (_lock)
            {
                this.InternalValidate();

                _objs.Push(obj);
            }
        }

        public void Add(params IDisposable[] objs)
        {
            lock (_lock)
            {
                this.InternalValidate();

                foreach (var item in objs)
                {
                    _objs.Push(item);
                }
            }
        }

        void InternalValidate()
        {
            if (_objs == null)
                throw new InvalidOperationException();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                this.InternalValidate();

                while (_objs.Count > 0)
                {
                    try
                    {
                        _objs.Pop().Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                }
            }
        }
    }
}
