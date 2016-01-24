using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    class Buffer<T> : IDisposable
        where T : struct
    {
        Type _itemType;
        Int32 _itemSize;

        Int32 _length;
        IntPtr _ptr;
        IntPtr _end;

        public IntPtr Start { get { return _ptr; } }
        public IntPtr End { get { return _end; } }

        public Buffer(int count)
        {
            _itemType = typeof(T);
            _itemSize = Marshal.SizeOf(_itemType);

            _length = _itemSize * count;
            _ptr = Marshal.AllocHGlobal(_length);
            _end = _ptr + _length;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_ptr);
            _ptr = IntPtr.Zero;
        }
    }
}
