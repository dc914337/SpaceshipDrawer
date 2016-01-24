using System;
using System.Runtime.Serialization;

namespace SpaceshipDrawer.Common
{
    [Serializable]
    public class AppException : Exception
    {
        public object State { get; private set; }

        public AppException() { this.State = null; }
        public AppException(object state) { this.State = state; }
        public AppException(string message) : base(message) { this.State = null; }
        public AppException(object state, string message) : base(message) { this.State = state; }
        public AppException(string format, params object[] args) : base(string.Format(format, args)) { this.State = null; }
        public AppException(object state, string format, params object[] args) : base(string.Format(format, args)) { this.State = state; }
        public AppException(Exception inner, string message) : base(message, inner) { this.State = null; }
        public AppException(Exception inner, string format, params object[] args) : base(string.Format(format, args), inner) { this.State = null; }
        public AppException(Exception inner, object state, string format, params object[] args) : base(string.Format(format, args), inner) { this.State = state; }

        protected AppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.State = info.GetValue("appState", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("appState", this.State);
            base.GetObjectData(info, context);
        }
    }
}
