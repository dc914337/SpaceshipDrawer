using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Color4f
    {
        public static readonly Color4f Zero = new Color4f(0, 0, 0, 0);

        public float A;
        public float B;
        public float G;
        public float R;

        public Color4f(float r, float g, float b, float a = 1.0f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static implicit operator Color4b(Color4f c)
        {
            return new Color4b(
                (byte)(c.R * 255),
                (byte)(c.G * 255),
                (byte)(c.B * 255),
                (byte)(c.A * 255)
            );
        }

        public static Color4f operator +(Color4f a, Color4f b)
        {
            return new Color4f(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
        }

        public static Color4f operator -(Color4f a, Color4f b)
        {
            return new Color4f(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
        }

        public static Color4f operator *(Color4f v, float scale)
        {
            return new Color4f(v.R * scale, v.G * scale, v.B * scale, v.A * scale);
        }

        public static Color4f operator /(Color4f v, float scale)
        {
            return new Color4f(v.R / scale, v.G / scale, v.B / scale, v.A / scale);
        }

        public static Color4f operator *(float scale, Color4f v)
        {
            return new Color4f(v.R * scale, v.G * scale, v.B * scale, v.A * scale);
        }

        public static Color4f operator /(float scale, Color4f v)
        {
            return new Color4f(v.R / scale, v.G / scale, v.B / scale, v.A / scale);
        }

        public float Length()
        {
            float y = this.G;
            float x = this.R;
            float z = this.B;
            float w = this.A;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        public float LengthSquared()
        {
            float y = this.G;
            float x = this.R;
            float z = this.B;
            float w = this.A;
            return (float)((x * x) + (y * y) + (z * z) + (w * w));
        }

        public Color4f Normalize()
        {
            double y = this.G;
            double x = this.R;
            double z = this.B;
            double w = this.A;
            float num;
            float length = (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

            if (length != 0f)
            {
                num = (float)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Color4f(this.R * num, this.G * num, this.B * num, this.A * num);
        }

        public static bool operator ==(Color4f left, Color4f right)
        {
            return (left.R == right.R) && (left.G == right.G) && (left.B == right.B) && (left.A == right.A);
        }

        public static bool operator !=(Color4f left, Color4f right)
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

        public bool Equals(Color4f other)
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
            return this.Equals((Color4f)obj);
        }

    }
}
