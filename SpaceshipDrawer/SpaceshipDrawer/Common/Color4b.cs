using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Color4b
    {
        public static readonly Color4b Zero = new Color4b(0, 0, 0, 0);

        public byte A;
        public byte B;
        public byte G;
        public byte R;

        public Color4b(byte r, byte g, byte b, byte a = 255)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        //public static Color4f operator +(Color4f a, Color4f b)
        //{
        //    return new Color4f(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
        //}

        //public static Color4f operator -(Color4f a, Color4f b)
        //{
        //    return new Color4f(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
        //}

        //public static Color4f operator *(Color4f v, byte scale)
        //{
        //    return new Color4f(v.R * scale, v.G * scale, v.B * scale, v.A * scale);
        //}

        //public static Color4f operator /(Color4f v, byte scale)
        //{
        //    return new Color4f(v.R / scale, v.G / scale, v.B / scale, v.A / scale);
        //}

        //public static Color4f operator *(byte scale, Color4f v)
        //{
        //    return new Color4f(v.R * scale, v.G * scale, v.B * scale, v.A * scale);
        //}

        //public static Color4f operator /(byte scale, Color4f v)
        //{
        //    return new Color4f(v.R / scale, v.G / scale, v.B / scale, v.A / scale);
        //}

        //public byte Length()
        //{
        //    byte y = this.G;
        //    byte x = this.R;
        //    byte z = this.B;
        //    byte w = this.A;
        //    return (byte)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        //}

        //public byte LengthSquared()
        //{
        //    byte y = this.G;
        //    byte x = this.R;
        //    byte z = this.B;
        //    byte w = this.A;
        //    return (byte)((x * x) + (y * y) + (z * z) + (w * w));
        //}

        //public Color4f Normalize()
        //{
        //    double y = this.G;
        //    double x = this.R;
        //    double z = this.B;
        //    double w = this.A;
        //    byte num;
        //    byte length = (byte)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

        //    if (length != 0f)
        //    {
        //        num = (byte)(1.0 / ((double)length));
        //    }
        //    else
        //    {
        //        num = 1;
        //    }

        //    return new Color4f(this.R * num, this.G * num, this.B * num, this.A * num);
        //}

        public static bool operator ==(Color4b left, Color4b right)
        {
            return (left.R == right.R) && (left.G == right.G) && (left.B == right.B) && (left.A == right.A);
        }

        public static bool operator !=(Color4b left, Color4b right)
        {
            return (left.R != right.R) || (left.G != right.G) || (left.B != right.B) || (left.A != right.A);
        }

        public override string ToString()
        {
            return string.Format("R:{0} G:{1} B:{2} A:{3}", this.R, this.G, this.B, this.A);
        }

        public override int GetHashCode()
        {
            return this.R.GetHashCode() ^ this.G.GetHashCode() ^ this.B.GetHashCode() ^ this.A.GetHashCode();
        }

        public bool Equals(Color4b other)
        {
            return (this.R == other.R) && (this.G == other.G) && (this.B == other.B) && (this.A == other.A);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return this.Equals((Color4b)obj);
        }


        internal Color4f ToColor4f()
        {
            return new Color4f(this.R / 255.0f, this.G / 255.0f, this.B / 255.0f, this.A / 255.0f);
        }
    }
}
