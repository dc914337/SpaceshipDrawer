using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Quaternion4f : IEquatable<Quaternion4f>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion4f(Vec3f value, float w)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = w;
        }

        public Quaternion4f(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Quaternion4f Identity
        {
            get
            {
                return new Quaternion4f { X = 0f, Y = 0f, Z = 0f, W = 1f };
            }
        }

        public bool IsIdentity
        {
            get
            {
                return ((((this.X == 0f) && (this.Y == 0f)) && (this.Z == 0f)) && ((this.W != 1f) ? false : true));
            }
        }

        //public Vector3 Axis
        //{
        //    get
        //{
        //    float angle;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinThis = (ref Quaternion4f modopt(IsExplicitlyDereferenced)) this;
        //    Vector3 axis = new Vector3();
        //    D3DXQuaternionToAxisAngle((D3DXQUATERNION modopt(IsConst)*) ((int) pinThis), (D3DXVECTOR3*) ((int) &axis), &angle);
        //    return axis;
        //}
        //}
        //public float Angle
        //{
        //    get
        //{
        //    float angle;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinThis = (ref Quaternion4f modopt(IsExplicitlyDereferenced)) this;
        //    Vector3 axis = new Vector3();
        //    D3DXQuaternionToAxisAngle((D3DXQUATERNION modopt(IsConst)*) ((int) pinThis), (D3DXVECTOR3*) ((int) &axis), &angle);
        //    return angle;
        //}
        //}

        public float Length()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            return (float)Math.Sqrt((((x * x) + (y * y)) + (z * z)) + (w * w));
        }

        public float LengthSquared()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            return (float)((((x * x) + (y * y)) + (z * z)) + (w * w));
        }

        public static void Normalize(ref Quaternion4f quaternion, out Quaternion4f result)
        {
            float length = (float)(1.0 / ((double)quaternion.Length()));
            result.X = quaternion.X * length;
            result.Y = quaternion.Y * length;
            result.Z = quaternion.Z * length;
            result.W = quaternion.W * length;
        }

        public static Quaternion4f Normalize(Quaternion4f quaternion)
        {
            quaternion.Normalize();
            return quaternion;
        }

        public void Normalize()
        {
            float length = (float)(1.0 / ((double)this.Length()));
            this.X *= length;
            this.Y *= length;
            this.Z *= length;
            this.W *= length;
        }

        //    public static void Conjugate(ref Quaternion4f quaternion, out Quaternion4f result)
        //    {
        //        result.X = -quaternion.X;
        //        result.Y = -quaternion.Y;
        //        result.Z = -quaternion.Z;
        //        result.W = quaternion.W;
        //    }

        //    public static Quaternion4f Conjugate(Quaternion4f quaternion)
        //    {
        //        return new Quaternion4f { X = -quaternion.X, Y = -quaternion.Y, Z = -quaternion.Z, W = quaternion.W };
        //    }

        //    public void Conjugate()
        //    {
        //        this.X = -this.X;
        //        this.Y = -this.Y;
        //        this.Z = -this.Z;
        //    }

        //    public static void Invert(ref Quaternion4f quaternion, out Quaternion4f result)
        //    {
        //        double y = quaternion.Y;
        //        double x = quaternion.X;
        //        double z = quaternion.Z;
        //        double w = quaternion.W;
        //        float lengthSq = (float)(1.0 / ((((x * x) + (y * y)) + (z * z)) + (w * w)));
        //        result.X = -quaternion.X * lengthSq;
        //        result.Y = -quaternion.Y * lengthSq;
        //        result.Z = -quaternion.Z * lengthSq;
        //        result.W = quaternion.W * lengthSq;
        //    }

        //    public static Quaternion4f Invert(Quaternion4f quaternion)
        //    {
        //        Quaternion4f result = new Quaternion4f();
        //        double y = quaternion.Y;
        //        double x = quaternion.X;
        //        double z = quaternion.Z;
        //        double w = quaternion.W;
        //        float lengthSq = (float)(1.0 / ((((x * x) + (y * y)) + (z * z)) + (w * w)));
        //        result.X = -quaternion.X * lengthSq;
        //        result.Y = -quaternion.Y * lengthSq;
        //        result.Z = -quaternion.Z * lengthSq;
        //        result.W = quaternion.W * lengthSq;
        //        return result;
        //    }

        //    public void Invert()
        //    {
        //        double y = this.Y;
        //        double x = this.X;
        //        double z = this.Z;
        //        double w = this.W;
        //        float lengthSq = (float)(1.0 / ((((x * x) + (y * y)) + (z * z)) + (w * w)));
        //        this.X = -this.X * lengthSq;
        //        this.Y = -this.Y * lengthSq;
        //        this.Z = -this.Z * lengthSq;
        //        this.W *= lengthSq;
        //    }

        //    public static void Add(ref Quaternion4f left, ref Quaternion4f right, out Quaternion4f result)
        //    {
        //        Quaternion4f r = new Quaternion4f {
        //            X = left.X + right.X,
        //            Y = left.Y + right.Y,
        //            Z = left.Z + right.Z,
        //            W = left.W + right.W
        //        };
        //        result = r;
        //    }

        //    public static Quaternion4f Add(Quaternion4f left, Quaternion4f right)
        //    {
        //        return new Quaternion4f { X = left.X + right.X, Y = left.Y + right.Y, Z = left.Z + right.Z, W = left.W + right.W };
        //    }

        //    public static unsafe void Barycentric(ref Quaternion4f source1, ref Quaternion4f source2, ref Quaternion4f source3, float weight1, float weight2, out Quaternion4f result)
        //{
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pin1 = source1;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pin2 = source2;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pin3 = source3;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinResult = result;
        //    D3DXQuaternionBaryCentric((D3DXQUATERNION*) ((int) pinResult), (D3DXQUATERNION modopt(IsConst)*) ((int) pin1), (D3DXQUATERNION modopt(IsConst)*) ((int) pin2), (D3DXQUATERNION modopt(IsConst)*) ((int) pin3), weight1, weight2);
        //}

        //    public static unsafe Quaternion4f Barycentric(Quaternion4f source1, Quaternion4f source2, Quaternion4f source3, float weight1, float weight2)
        //{
        //    Quaternion4f result = new Quaternion4f();
        //    D3DXQuaternionBaryCentric((D3DXQUATERNION*) ((int) &result), (D3DXQUATERNION modopt(IsConst)*) ((int) &source1), (D3DXQUATERNION modopt(IsConst)*) ((int) &source2), (D3DXQUATERNION modopt(IsConst)*) ((int) &source3), weight1, weight2);
        //    return result;
        //}

        //    public static void Divide(ref Quaternion4f left, ref Quaternion4f right, out Quaternion4f result)
        //    {
        //        result.X = (float)(((double)left.X) / ((double)right.X));
        //        result.Y = (float)(((double)left.Y) / ((double)right.Y));
        //        result.Z = (float)(((double)left.Z) / ((double)right.Z));
        //        result.W = (float)(((double)left.W) / ((double)right.W));
        //    }

        //    public static Quaternion4f Divide(Quaternion4f left, Quaternion4f right)
        //    {
        //        return new Quaternion4f { X = (float)(((double)left.X) / ((double)right.X)), Y = (float)(((double)left.Y) / ((double)right.Y)), Z = (float)(((double)left.Z) / ((double)right.Z)), W = (float)(((double)left.W) / ((double)right.W)) };
        //    }

        //    public static float Dot(Quaternion4f left, Quaternion4f right)
        //    {
        //        return ((((left.Y * right.Y) + (left.X * right.X)) + (left.Z * right.Z)) + (left.W * right.W));
        //    }

        //    public static unsafe void Exponential(ref Quaternion4f quaternion, out Quaternion4f result)
        //{
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinQuat = quaternion;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinResult = result;
        //    D3DXQuaternionExp((D3DXQUATERNION*) ((int) pinResult), (D3DXQUATERNION modopt(IsConst)*) ((int) pinQuat));
        //}

        //    public static unsafe Quaternion4f Exponential(Quaternion4f quaternion)
        //{
        //    Quaternion4f result = new Quaternion4f();
        //    D3DXQuaternionExp((D3DXQUATERNION*) ((int) &result), (D3DXQUATERNION modopt(IsConst)*) ((int) &quaternion));
        //    return result;
        //}

        //    public static void Lerp(ref Quaternion4f start, ref Quaternion4f end, float amount, out Quaternion4f result)
        //    {
        //        float inverse = ((float)1.0) - amount;
        //        if (((((start.Y * end.Y) + (start.X * end.X)) + (start.Z * end.Z)) + (start.W * end.W)) >= 0f)
        //        {
        //            result.X = (start.X * inverse) + (end.X * amount);
        //            result.Y = (start.Y * inverse) + (end.Y * amount);
        //            result.Z = (start.Z * inverse) + (end.Z * amount);
        //            result.W = (start.W * inverse) + (end.W * amount);
        //        }
        //        else
        //        {
        //            result.X = (start.X * inverse) - (end.X * amount);
        //            result.Y = (start.Y * inverse) - (end.Y * amount);
        //            result.Z = (start.Z * inverse) - (end.Z * amount);
        //            result.W = (start.W * inverse) - (end.W * amount);
        //        }
        //        float invLength = (float)(1.0 / ((double)result.Length()));
        //        result.X *= invLength;
        //        result.Y *= invLength;
        //        result.Z *= invLength;
        //        result.W *= invLength;
        //    }

        //    public static Quaternion4f Lerp(Quaternion4f start, Quaternion4f end, float amount)
        //    {
        //        Quaternion4f result = new Quaternion4f();
        //        float inverse = ((float)1.0) - amount;
        //        if (((((start.Y * end.Y) + (start.X * end.X)) + (start.Z * end.Z)) + (start.W * end.W)) >= 0f)
        //        {
        //            result.X = (start.X * inverse) + (end.X * amount);
        //            result.Y = (start.Y * inverse) + (end.Y * amount);
        //            result.Z = (start.Z * inverse) + (end.Z * amount);
        //            result.W = (start.W * inverse) + (end.W * amount);
        //        }
        //        else
        //        {
        //            result.X = (start.X * inverse) - (end.X * amount);
        //            result.Y = (start.Y * inverse) - (end.Y * amount);
        //            result.Z = (start.Z * inverse) - (end.Z * amount);
        //            result.W = (start.W * inverse) - (end.W * amount);
        //        }
        //        float invLength = (float)(1.0 / ((double)result.Length()));
        //        result.X *= invLength;
        //        result.Y *= invLength;
        //        result.Z *= invLength;
        //        result.W *= invLength;
        //        return result;
        //    }

        //    public static unsafe void Logarithm(ref Quaternion4f quaternion, out Quaternion4f result)
        //{
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinQuat = quaternion;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinResult = result;
        //    D3DXQuaternionLn((D3DXQUATERNION*) ((int) pinResult), (D3DXQUATERNION modopt(IsConst)*) ((int) pinQuat));
        //}

        //    public static unsafe Quaternion4f Logarithm(Quaternion4f quaternion)
        //{
        //    Quaternion4f result = new Quaternion4f();
        //    D3DXQuaternionLn((D3DXQUATERNION*) ((int) &result), (D3DXQUATERNION modopt(IsConst)*) ((int) &quaternion));
        //    return result;
        //}

        //    public static void Multiply(ref Quaternion4f quaternion, float scale, out Quaternion4f result)
        //    {
        //        result.X = quaternion.X * scale;
        //        result.Y = quaternion.Y * scale;
        //        result.Z = quaternion.Z * scale;
        //        result.W = quaternion.W * scale;
        //    }

        //    public static Quaternion4f Multiply(Quaternion4f quaternion, float scale)
        //    {
        //        return new Quaternion4f { X = quaternion.X * scale, Y = quaternion.Y * scale, Z = quaternion.Z * scale, W = quaternion.W * scale };
        //    }

        //    public static void Multiply(ref Quaternion4f left, ref Quaternion4f right, out Quaternion4f result)
        //    {
        //        float lx = left.X;
        //        float ly = left.Y;
        //        float lz = left.Z;
        //        float lw = left.W;
        //        float rx = right.X;
        //        float ry = right.Y;
        //        float rz = right.Z;
        //        float rw = right.W;
        //        result.X = (((rx * lw) + (rw * lx)) + (ry * lz)) - (rz * ly);
        //        result.Y = (((ry * lw) + (rw * ly)) + (rz * lx)) - (rx * lz);
        //        result.Z = (((rz * lw) + (rw * lz)) + (rx * ly)) - (ry * lx);
        //        result.W = (rw * lw) - (((ry * ly) + (rx * lx)) + (rz * lz));
        //    }

        //    public static Quaternion4f Multiply(Quaternion4f left, Quaternion4f right)
        //    {
        //        Quaternion4f quaternion = new Quaternion4f();
        //        float lx = left.X;
        //        float ly = left.Y;
        //        float lz = left.Z;
        //        float lw = left.W;
        //        float rx = right.X;
        //        float ry = right.Y;
        //        float rz = right.Z;
        //        float rw = right.W;
        //        quaternion.X = (((rx * lw) + (rw * lx)) + (ry * lz)) - (rz * ly);
        //        quaternion.Y = (((ry * lw) + (rw * ly)) + (rz * lx)) - (rx * lz);
        //        quaternion.Z = (((rz * lw) + (rw * lz)) + (rx * ly)) - (ry * lx);
        //        quaternion.W = (rw * lw) - (((ry * ly) + (rx * lx)) + (rz * lz));
        //        return quaternion;
        //    }

        //    public static void Negate(ref Quaternion4f quaternion, out Quaternion4f result)
        //    {
        //        result.X = -quaternion.X;
        //        result.Y = -quaternion.Y;
        //        result.Z = -quaternion.Z;
        //        result.W = -quaternion.W;
        //    }

        //    public static Quaternion4f Negate(Quaternion4f quaternion)
        //    {
        //        return new Quaternion4f { X = -quaternion.X, Y = -quaternion.Y, Z = -quaternion.Z, W = -quaternion.W };
        //    }

        //    public static void RotationAxis(ref Vector3 axis, float angle, out Quaternion4f result)
        //    {
        //        Vector3.Normalize(ref axis, out axis);
        //        float half = angle * 0.5f;
        //        float sin = (float)Math.Sin((double)half);
        //        float cos = (float)Math.Cos((double)half);
        //        result.X = axis.X * sin;
        //        result.Y = axis.Y * sin;
        //        result.Z = axis.Z * sin;
        //        result.W = cos;
        //    }

        //    public static Quaternion4f RotationAxis(Vector3 axis, float angle)
        //    {
        //        Quaternion4f result = new Quaternion4f();
        //        Vector3.Normalize(ref axis, out axis);
        //        float half = angle * 0.5f;
        //        float sin = (float)Math.Sin((double)half);
        //        float cos = (float)Math.Cos((double)half);
        //        result.X = axis.X * sin;
        //        result.Y = axis.Y * sin;
        //        result.Z = axis.Z * sin;
        //        result.W = cos;
        //        return result;
        //    }

        //    public static void RotationMatrix(ref Matrix matrix, out Quaternion4f result)
        //    {
        //        float scale = (matrix.M22 + matrix.M11) + matrix.M33;
        //        if (scale > 0f)
        //        {
        //            float sqrt = (float)Math.Sqrt(scale + 1.0);
        //            result.W = sqrt * 0.5f;
        //            sqrt = (float)(0.5 / ((double)sqrt));
        //            result.X = (matrix.M23 - matrix.M32) * sqrt;
        //            result.Y = (matrix.M31 - matrix.M13) * sqrt;
        //            result.Z = (matrix.M12 - matrix.M21) * sqrt;
        //        }
        //        else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
        //        {
        //            float sqrt = (float)Math.Sqrt(((matrix.M11 + 1.0) - matrix.M22) - matrix.M33);
        //            float half = (float)(0.5 / ((double)sqrt));
        //            result.X = sqrt * 0.5f;
        //            result.Y = (matrix.M21 + matrix.M12) * half;
        //            result.Z = (matrix.M13 + matrix.M31) * half;
        //            result.W = (matrix.M23 - matrix.M32) * half;
        //        }
        //        else if (matrix.M22 > matrix.M33)
        //        {
        //            float sqrt = (float)Math.Sqrt(((matrix.M22 + 1.0) - matrix.M11) - matrix.M33);
        //            float half = (float)(0.5 / ((double)sqrt));
        //            result.X = (matrix.M21 + matrix.M12) * half;
        //            result.Y = sqrt * 0.5f;
        //            result.Z = (matrix.M32 + matrix.M23) * half;
        //            result.W = (matrix.M31 - matrix.M13) * half;
        //        }
        //        else
        //        {
        //            float sqrt = (float)Math.Sqrt(((matrix.M33 + 1.0) - matrix.M11) - matrix.M22);
        //            float half = (float)(0.5 / ((double)sqrt));
        //            result.X = (matrix.M13 + matrix.M31) * half;
        //            result.Y = (matrix.M32 + matrix.M23) * half;
        //            result.Z = sqrt * 0.5f;
        //            result.W = (matrix.M12 - matrix.M21) * half;
        //        }
        //    }

        //    public static Quaternion4f RotationMatrix(Matrix matrix)
        //    {
        //        Quaternion4f result = new Quaternion4f();
        //        float scale = (matrix.M22 + matrix.M11) + matrix.M33;
        //        if (scale > 0f)
        //        {
        //            float sqrt = (float)Math.Sqrt(scale + 1.0);
        //            result.W = sqrt * 0.5f;
        //            sqrt = (float)(0.5 / ((double)sqrt));
        //            result.X = (matrix.M23 - matrix.M32) * sqrt;
        //            result.Y = (matrix.M31 - matrix.M13) * sqrt;
        //            result.Z = (matrix.M12 - matrix.M21) * sqrt;
        //            return result;
        //        }
        //        if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
        //        {
        //            float sqrt = (float)Math.Sqrt(((matrix.M11 + 1.0) - matrix.M22) - matrix.M33);
        //            float half = (float)(0.5 / ((double)sqrt));
        //            result.X = sqrt * 0.5f;
        //            result.Y = (matrix.M21 + matrix.M12) * half;
        //            result.Z = (matrix.M13 + matrix.M31) * half;
        //            result.W = (matrix.M23 - matrix.M32) * half;
        //            return result;
        //        }
        //        if (matrix.M22 > matrix.M33)
        //        {
        //            float sqrt = (float)Math.Sqrt(((matrix.M22 + 1.0) - matrix.M11) - matrix.M33);
        //            float half = (float)(0.5 / ((double)sqrt));
        //            result.X = (matrix.M21 + matrix.M12) * half;
        //            result.Y = sqrt * 0.5f;
        //            result.Z = (matrix.M32 + matrix.M23) * half;
        //            result.W = (matrix.M31 - matrix.M13) * half;
        //            return result;
        //        }
        //        float sqrt = (float)Math.Sqrt(((matrix.M33 + 1.0) - matrix.M11) - matrix.M22);
        //        float half = (float)(0.5 / ((double)sqrt));
        //        result.X = (matrix.M13 + matrix.M31) * half;
        //        result.Y = (matrix.M32 + matrix.M23) * half;
        //        result.Z = sqrt * 0.5f;
        //        result.W = (matrix.M12 - matrix.M21) * half;
        //        return result;
        //    }

        //    public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Quaternion4f result)
        //    {
        //        float halfRoll = roll * 0.5f;
        //        float sinRoll = (float)Math.Sin((double)halfRoll);
        //        float cosRoll = (float)Math.Cos((double)halfRoll);
        //        float halfPitch = pitch * 0.5f;
        //        float sinPitch = (float)Math.Sin((double)halfPitch);
        //        float cosPitch = (float)Math.Cos((double)halfPitch);
        //        float halfYaw = yaw * 0.5f;
        //        float sinYaw = (float)Math.Sin((double)halfYaw);
        //        float cosYaw = (float)Math.Cos((double)halfYaw);
        //        double num4 = cosYaw * sinPitch;
        //        double num3 = sinYaw * cosPitch;
        //        result.X = (float)((sinRoll * num3) + (cosRoll * num4));
        //        result.Y = (float)((cosRoll * num3) - (sinRoll * num4));
        //        double num2 = cosYaw * cosPitch;
        //        double num = sinYaw * sinPitch;
        //        result.Z = (float)((sinRoll * num2) - (cosRoll * num));
        //        result.W = (float)((sinRoll * num) + (cosRoll * num2));
        //    }

        public static Quaternion4f RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            float halfRoll = roll * 0.5f;
            float sinRoll = (float)Math.Sin((double)halfRoll);
            float cosRoll = (float)Math.Cos((double)halfRoll);
            float halfPitch = pitch * 0.5f;
            float sinPitch = (float)Math.Sin((double)halfPitch);
            float cosPitch = (float)Math.Cos((double)halfPitch);
            float halfYaw = yaw * 0.5f;
            float sinYaw = (float)Math.Sin((double)halfYaw);
            float cosYaw = (float)Math.Cos((double)halfYaw);
            double num4 = cosYaw * sinPitch;
            double num3 = sinYaw * cosPitch;
            double num2 = cosYaw * cosPitch;
            double num = sinYaw * sinPitch;

            return new Quaternion4f(
                (float)((sinRoll * num3) + (cosRoll * num4)),
                (float)((cosRoll * num3) - (sinRoll * num4)),
                (float)((sinRoll * num2) - (cosRoll * num)),
                (float)((sinRoll * num) + (cosRoll * num2))
            );
        }

        //    public static void Slerp(ref Quaternion4f start, ref Quaternion4f end, float amount, out Quaternion4f result)
        //    {
        //        float opposite;
        //        float inverse;
        //        float dot = (((start.Y * end.Y) + (start.X * end.X)) + (start.Z * end.Z)) + (start.W * end.W);
        //        bool flag = false;
        //        if (dot < 0f)
        //        {
        //            flag = true;
        //            dot = -dot;
        //        }
        //        if (dot > 0.99999898672103882)
        //        {
        //            float num2;
        //            inverse = ((float)1.0) - amount;
        //            if (flag)
        //            {
        //                num2 = -amount;
        //            }
        //            else
        //            {
        //                num2 = amount;
        //            }
        //            opposite = num2;
        //        }
        //        else
        //        {
        //            float num;
        //            float acos = (float)Math.Acos((double)dot);
        //            float invSin = (float)(1.0 / Math.Sin((double)acos));
        //            inverse = ((float)Math.Sin((1.0 - amount) * acos)) * invSin;
        //            if (flag)
        //            {
        //                num = ((float)-Math.Sin(acos * amount)) * invSin;
        //            }
        //            else
        //            {
        //                num = ((float)Math.Sin(acos * amount)) * invSin;
        //            }
        //            opposite = num;
        //        }
        //        result.X = (end.X * opposite) + (start.X * inverse);
        //        result.Y = (end.Y * opposite) + (start.Y * inverse);
        //        result.Z = (end.Z * opposite) + (start.Z * inverse);
        //        result.W = (end.W * opposite) + (start.W * inverse);
        //    }

        //    public static Quaternion4f Slerp(Quaternion4f start, Quaternion4f end, float amount)
        //    {
        //        float opposite;
        //        float inverse;
        //        Quaternion4f result = new Quaternion4f();
        //        float dot = (((start.Y * end.Y) + (start.X * end.X)) + (start.Z * end.Z)) + (start.W * end.W);
        //        bool flag = false;
        //        if (dot < 0f)
        //        {
        //            flag = true;
        //            dot = -dot;
        //        }
        //        if (dot > 0.99999898672103882)
        //        {
        //            float num2;
        //            inverse = ((float)1.0) - amount;
        //            if (flag)
        //            {
        //                num2 = -amount;
        //            }
        //            else
        //            {
        //                num2 = amount;
        //            }
        //            opposite = num2;
        //        }
        //        else
        //        {
        //            float num;
        //            float acos = (float)Math.Acos((double)dot);
        //            float invSin = (float)(1.0 / Math.Sin((double)acos));
        //            inverse = ((float)Math.Sin((1.0 - amount) * acos)) * invSin;
        //            if (flag)
        //            {
        //                num = ((float)-Math.Sin(acos * amount)) * invSin;
        //            }
        //            else
        //            {
        //                num = ((float)Math.Sin(acos * amount)) * invSin;
        //            }
        //            opposite = num;
        //        }
        //        result.X = (end.X * opposite) + (start.X * inverse);
        //        result.Y = (end.Y * opposite) + (start.Y * inverse);
        //        result.Z = (end.Z * opposite) + (start.Z * inverse);
        //        result.W = (end.W * opposite) + (start.W * inverse);
        //        return result;
        //    }

        //    public static unsafe void Squad(ref Quaternion4f source1, ref Quaternion4f source2, ref Quaternion4f source3, ref Quaternion4f source4, float amount, out Quaternion4f result)
        //{
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pin1 = source1;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinA = source2;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinB = source3;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinC = source4;
        //    ref Quaternion4f modopt(IsExplicitlyDereferenced) pinned pinResult = result;
        //    D3DXQuaternionSquad((D3DXQUATERNION*) ((int) pinResult), (D3DXQUATERNION modopt(IsConst)*) ((int) pin1), (D3DXQUATERNION modopt(IsConst)*) ((int) pinA), (D3DXQUATERNION modopt(IsConst)*) ((int) pinB), (D3DXQUATERNION modopt(IsConst)*) ((int) pinC), amount);
        //}

        //    public static unsafe Quaternion4f Squad(Quaternion4f source1, Quaternion4f source2, Quaternion4f source3, Quaternion4f source4, float amount)
        //{
        //    Quaternion4f result = new Quaternion4f();
        //    D3DXQuaternionSquad((D3DXQUATERNION*) ((int) &result), (D3DXQUATERNION modopt(IsConst)*) ((int) &source1), (D3DXQUATERNION modopt(IsConst)*) ((int) &source2), (D3DXQUATERNION modopt(IsConst)*) ((int) &source3), (D3DXQUATERNION modopt(IsConst)*) ((int) &source4), amount);
        //    return result;
        //}

        //    public static unsafe Quaternion4f[] SquadSetup(Quaternion4f source1, Quaternion4f source2, Quaternion4f source3, Quaternion4f source4)
        //{
        //    Quaternion4f result1 = new Quaternion4f();
        //    Quaternion4f result2 = new Quaternion4f();
        //    Quaternion4f result3 = new Quaternion4f();
        //    Quaternion4f[] results = new Quaternion4f[3];
        //    D3DXQuaternionSquadSetup((D3DXQUATERNION*) ((int) &result1), (D3DXQUATERNION*) ((int) &result2), (D3DXQUATERNION*) ((int) &result3), (D3DXQUATERNION modopt(IsConst)*) ((int) &source1), (D3DXQUATERNION modopt(IsConst)*) ((int) &source2), (D3DXQUATERNION modopt(IsConst)*) ((int) &source3), (D3DXQUATERNION modopt(IsConst)*) ((int) &source4));
        //    results[0] = result1;
        //    results[1] = result2;
        //    results[2] = result3;
        //    return results;
        //}

        //    public static void Subtract(ref Quaternion4f left, ref Quaternion4f right, out Quaternion4f result)
        //    {
        //        result.X = left.X - right.X;
        //        result.Y = left.Y - right.Y;
        //        result.Z = left.Z - right.Z;
        //        result.W = left.W - right.W;
        //    }

        //    public static Quaternion4f Subtract(Quaternion4f left, Quaternion4f right)
        //    {
        //        return new Quaternion4f { X = left.X - right.X, Y = left.Y - right.Y, Z = left.Z - right.Z, W = left.W - right.W };
        //    }

        public static Quaternion4f operator *(float scale, Quaternion4f quaternion)
        {
            return new Quaternion4f { X = quaternion.X * scale, Y = quaternion.Y * scale, Z = quaternion.Z * scale, W = quaternion.W * scale };
        }

        public static Quaternion4f operator *(Quaternion4f quaternion, float scale)
        {
            return new Quaternion4f { X = quaternion.X * scale, Y = quaternion.Y * scale, Z = quaternion.Z * scale, W = quaternion.W * scale };
        }

        public static Quaternion4f operator *(Quaternion4f left, Quaternion4f right)
        {
            Quaternion4f quaternion = new Quaternion4f();
            float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;
            quaternion.X = (((rx * lw) + (rw * lx)) + (ry * lz)) - (rz * ly);
            quaternion.Y = (((ry * lw) + (rw * ly)) + (rz * lx)) - (rx * lz);
            quaternion.Z = (((rz * lw) + (rw * lz)) + (rx * ly)) - (ry * lx);
            quaternion.W = (rw * lw) - (((ry * ly) + (rx * lx)) + (rz * lz));
            return quaternion;
        }

        public static Quaternion4f operator /(Quaternion4f left, float right)
        {
            return new Quaternion4f { X = (float)(((double)left.X) / ((double)right)), Y = (float)(((double)left.Y) / ((double)right)), Z = (float)(((double)left.Z) / ((double)right)), W = (float)(((double)left.W) / ((double)right)) };
        }

        public static Quaternion4f operator +(Quaternion4f left, Quaternion4f right)
        {
            return new Quaternion4f { X = left.X + right.X, Y = left.Y + right.Y, Z = left.Z + right.Z, W = left.W + right.W };
        }

        public static Quaternion4f operator -(Quaternion4f quaternion)
        {
            return new Quaternion4f { X = -quaternion.X, Y = -quaternion.Y, Z = -quaternion.Z, W = -quaternion.W };
        }

        public static Quaternion4f operator -(Quaternion4f left, Quaternion4f right)
        {
            return new Quaternion4f { X = left.X - right.X, Y = left.Y - right.Y, Z = left.Z - right.Z, W = left.W - right.W };
        }

        public static bool operator ==(Quaternion4f left, Quaternion4f right)
        {
            return Equals(ref left, ref right);
        }

        public static bool operator !=(Quaternion4f left, Quaternion4f right)
        {
            return !Equals(ref left, ref right);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2} W:{3}", this.X, this.Y, this.Z, this.W);
        }

        public override int GetHashCode()
        {
            float x = this.X;
            float y = this.Y;
            float z = this.Z;
            float w = this.W;
            int num = (z.GetHashCode() + w.GetHashCode()) + y.GetHashCode();
            return (x.GetHashCode() + num);
        }

        public static bool Equals(ref Quaternion4f value1, ref Quaternion4f value2)
        {
            return (((value1.X == value2.X) && (value1.Y == value2.Y)) && ((value1.Z == value2.Z) && (value1.W == value2.W)));
        }

        public bool Equals(Quaternion4f other)
        {
            return (((this.X == other.X) && (this.Y == other.Y)) && ((this.Z == other.Z) && (this.W == other.W)));
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
            return this.Equals((Quaternion4f)obj);
        }
    }

}
