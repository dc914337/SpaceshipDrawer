using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3f : IEquatable<Vec3f>
    {
        public static readonly Vec3f Zero = new Vec3f(0, 0, 0);
        public static readonly Vec3f I = new Vec3f(1, 0, 0);

        public float X;
        public float Y;
        public float Z;

        public Vec3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vec4f Complement(float w = 0)
        {
            return new Vec4f(this.X, this.Y, this.Z, w);
        }
        
        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3f operator *(Vec3f v, float scale)
        {
            return new Vec3f(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Vec3f operator /(Vec3f v, float scale)
        {
            return new Vec3f(v.X / scale, v.Y / scale, v.Z / scale);
        }

        public static Vec3f operator *(float scale, Vec3f v)
        {
            return new Vec3f(v.X * scale, v.Y * scale, v.Z * scale);
        }

        //public static Vec3f operator /(float scale, Vec3f v)
        //{
        //    return new Vec3f(v.X / scale, v.Y / scale, v.Z / scale);
        //}

        public static Vec3f operator %(Vec3f v, float k)
        {
            return new Vec3f(v.X % k, v.Y % k, v.Z % k);
        }

        public float Length()
        {
            float y = this.Y;
            float x = this.X;
            float z = this.Z;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public float LengthSquared()
        {
            float y = this.Y;
            float x = this.X;
            float z = this.Z;
            return (float)((x * x) + (y * y) + (z * z));
        }

        public Vec3f Normalize()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            float num;
            float length = (float)Math.Sqrt((x * x) + (y * y) + (z * z));

            if (length != 0f)
            {
                num = (float)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec3f(this.X * num, this.Y * num, this.Z * num);
        }

        public static bool operator ==(Vec3f left, Vec3f right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z);
        }

        public static bool operator !=(Vec3f left, Vec3f right)
        {
            return (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2}", this.X, this.Y, this.Z);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public bool Equals(Vec3f other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
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
            return this.Equals((Vec3f)obj);
        }


        //public static void Barycentric(ref Vec3f value1, ref Vec3f value2, ref Vec3f value3, float amount1, float amount2, out Vec3f result)
        //{
        //    Vec3f vector;
        //    vector.X = (((value2.X - value1.X) * amount1) + value1.X) + ((value3.X - value1.X) * amount2);
        //    vector.Y = (((value2.Y - value1.Y) * amount1) + value1.Y) + ((value3.Y - value1.Y) * amount2);
        //    vector.Z = (((value2.Z - value1.Z) * amount1) + value1.Z) + ((value3.Z - value1.Z) * amount2);
        //    result = vector;
        //}

        //public static Vec3f Barycentric(Vec3f value1, Vec3f value2, Vec3f value3, float amount1, float amount2)
        //{
        //    return new Vec3f { X = (((value2.X - value1.X) * amount1) + value1.X) + ((value3.X - value1.X) * amount2), Y = (((value2.Y - value1.Y) * amount1) + value1.Y) + ((value3.Y - value1.Y) * amount2), Z = (((value2.Z - value1.Z) * amount1) + value1.Z) + ((value3.Z - value1.Z) * amount2) };
        //}

        //public static void CatmullRom(ref Vec3f value1, ref Vec3f value2, ref Vec3f value3, ref Vec3f value4, float amount, out Vec3f result)
        //{
        //    double num = amount;
        //    float squared = (float)(num * num);
        //    float cubed = squared * amount;
        //    Vec3f r = new Vec3f {
        //        X = (float)((((((((value1.X * 2.0) - (value2.X * 5.0)) + (value3.X * 4.0)) - value4.X) * squared) + (((value3.X - value1.X) * amount) + (value2.X * 2.0))) + (((((value2.X * 3.0) - value1.X) - (value3.X * 3.0)) + value4.X) * cubed)) * 0.5),
        //        Y = (float)((((((((value1.Y * 2.0) - (value2.Y * 5.0)) + (value3.Y * 4.0)) - value4.Y) * squared) + (((value3.Y - value1.Y) * amount) + (value2.Y * 2.0))) + (((((value2.Y * 3.0) - value1.Y) - (value3.Y * 3.0)) + value4.Y) * cubed)) * 0.5),
        //        Z = (float)((((((((value1.Z * 2.0) - (value2.Z * 5.0)) + (value3.Z * 4.0)) - value4.Z) * squared) + (((value3.Z - value1.Z) * amount) + (value2.Z * 2.0))) + (((((value2.Z * 3.0) - value1.Z) - (value3.Z * 3.0)) + value4.Z) * cubed)) * 0.5)
        //    };
        //    result = r;
        //}

        //public static Vec3f CatmullRom(Vec3f value1, Vec3f value2, Vec3f value3, Vec3f value4, float amount)
        //{
        //    Vec3f vector = new Vector3();
        //    double num = amount;
        //    float squared = (float)(num * num);
        //    float cubed = squared * amount;
        //    vector.X = (float)((((((((value1.X * 2.0) - (value2.X * 5.0)) + (value3.X * 4.0)) - value4.X) * squared) + (((value3.X - value1.X) * amount) + (value2.X * 2.0))) + (((((value2.X * 3.0) - value1.X) - (value3.X * 3.0)) + value4.X) * cubed)) * 0.5);
        //    vector.Y = (float)((((((((value1.Y * 2.0) - (value2.Y * 5.0)) + (value3.Y * 4.0)) - value4.Y) * squared) + (((value3.Y - value1.Y) * amount) + (value2.Y * 2.0))) + (((((value2.Y * 3.0) - value1.Y) - (value3.Y * 3.0)) + value4.Y) * cubed)) * 0.5);
        //    vector.Z = (float)((((((((value1.Z * 2.0) - (value2.Z * 5.0)) + (value3.Z * 4.0)) - value4.Z) * squared) + (((value3.Z - value1.Z) * amount) + (value2.Z * 2.0))) + (((((value2.Z * 3.0) - value1.Z) - (value3.Z * 3.0)) + value4.Z) * cubed)) * 0.5);
        //    return vector;
        //}

        //public static void Clamp(ref Vec3f value, ref Vec3f min, ref Vec3f max, out Vec3f result)
        //{
        //    float num;
        //    float num2;
        //    float num3;
        //    float num4;
        //    float num5;
        //    float num6;
        //    Vec3f vector;
        //    float x = value.X;
        //    if (x > max.X)
        //    {
        //        num3 = max.X;
        //    }
        //    else
        //    {
        //        num3 = x;
        //    }
        //    if (num3 < min.X)
        //    {
        //        num6 = min.X;
        //    }
        //    else
        //    {
        //        num6 = num3;
        //    }
        //    float y = value.Y;
        //    if (y > max.Y)
        //    {
        //        num2 = max.Y;
        //    }
        //    else
        //    {
        //        num2 = y;
        //    }
        //    if (num2 < min.Y)
        //    {
        //        num5 = min.Y;
        //    }
        //    else
        //    {
        //        num5 = num2;
        //    }
        //    float z = value.Z;
        //    if (z > max.Z)
        //    {
        //        num = max.Z;
        //    }
        //    else
        //    {
        //        num = z;
        //    }
        //    if (num < min.Z)
        //    {
        //        num4 = min.Z;
        //    }
        //    else
        //    {
        //        num4 = num;
        //    }
        //    vector.X = num6;
        //    vector.Y = num5;
        //    vector.Z = num4;
        //    result = vector;
        //}

        //public static Vec3f Clamp(Vec3f value, Vec3f min, Vec3f max)
        //{
        //    float num;
        //    float num2;
        //    float num3;
        //    float num4;
        //    float num5;
        //    float num6;
        //    Vec3f vector;
        //    float x = value.X;
        //    if (x > max.X)
        //    {
        //        num3 = max.X;
        //    }
        //    else
        //    {
        //        num3 = x;
        //    }
        //    if (num3 < min.X)
        //    {
        //        num6 = min.X;
        //    }
        //    else
        //    {
        //        num6 = num3;
        //    }
        //    float y = value.Y;
        //    if (y > max.Y)
        //    {
        //        num2 = max.Y;
        //    }
        //    else
        //    {
        //        num2 = y;
        //    }
        //    if (num2 < min.Y)
        //    {
        //        num5 = min.Y;
        //    }
        //    else
        //    {
        //        num5 = num2;
        //    }
        //    float z = value.Z;
        //    if (z > max.Z)
        //    {
        //        num = max.Z;
        //    }
        //    else
        //    {
        //        num = z;
        //    }
        //    if (num < min.Z)
        //    {
        //        num4 = min.Z;
        //    }
        //    else
        //    {
        //        num4 = num;
        //    }
        //    vector.X = num6;
        //    vector.Y = num5;
        //    vector.Z = num4;
        //    return vector;
        //}

        //public static void Hermite(ref Vec3f value1, ref Vec3f tangent1, ref Vec3f value2, ref Vec3f tangent2, float amount, out Vec3f result)
        //{
        //    double num2 = amount;
        //    float squared = (float)(num2 * num2);
        //    float cubed = squared * amount;
        //    double num = squared * 3.0;
        //    float part1 = (float)(((cubed * 2.0) - num) + 1.0);
        //    float part2 = (float)((cubed * -2.0) + num);
        //    float part3 = (cubed - ((float)(squared * 2.0))) + amount;
        //    float part4 = cubed - squared;
        //    result.X = (((value2.X * part2) + (value1.X * part1)) + (tangent1.X * part3)) + (tangent2.X * part4);
        //    result.Y = (((value2.Y * part2) + (value1.Y * part1)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
        //    result.Z = (((value2.Z * part2) + (value1.Z * part1)) + (tangent1.Z * part3)) + (tangent2.Z * part4);
        //}

        //public static Vec3f Hermite(Vec3f value1, Vec3f tangent1, Vec3f value2, Vec3f tangent2, float amount)
        //{
        //    Vec3f vector = new Vector3();
        //    double num2 = amount;
        //    float squared = (float)(num2 * num2);
        //    float cubed = squared * amount;
        //    double num = squared * 3.0;
        //    float part1 = (float)(((cubed * 2.0) - num) + 1.0);
        //    float part2 = (float)((cubed * -2.0) + num);
        //    float part3 = (cubed - ((float)(squared * 2.0))) + amount;
        //    float part4 = cubed - squared;
        //    vector.X = (((value2.X * part2) + (value1.X * part1)) + (tangent1.X * part3)) + (tangent2.X * part4);
        //    vector.Y = (((value2.Y * part2) + (value1.Y * part1)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
        //    vector.Z = (((value2.Z * part2) + (value1.Z * part1)) + (tangent1.Z * part3)) + (tangent2.Z * part4);
        //    return vector;
        //}

        //public static void Lerp(ref Vec3f start, ref Vec3f end, float amount, out Vec3f result)
        //{
        //    result.X = ((end.X - start.X) * amount) + start.X;
        //    result.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    result.Z = ((end.Z - start.Z) * amount) + start.Z;
        //}

        //public static Vec3f Lerp(Vec3f start, Vec3f end, float amount)
        //{
        //    return new Vec3f { X = ((end.X - start.X) * amount) + start.X, Y = ((end.Y - start.Y) * amount) + start.Y, Z = ((end.Z - start.Z) * amount) + start.Z };
        //}

        //public static void SmoothStep(ref Vec3f start, ref Vec3f end, float amount, out Vec3f result)
        //{
        //    float num;
        //    if (amount > 1f)
        //    {
        //        num = 1f;
        //    }
        //    else
        //    {
        //        float num2;
        //        if (amount < 0f)
        //        {
        //            num2 = 0f;
        //        }
        //        else
        //        {
        //            num2 = amount;
        //        }
        //        num = num2;
        //    }
        //    double num3 = num;
        //    amount = (float)((3.0 - (num * 2.0)) * (num3 * num3));
        //    result.X = ((end.X - start.X) * amount) + start.X;
        //    result.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    result.Z = ((end.Z - start.Z) * amount) + start.Z;
        //}

        //public static Vec3f SmoothStep(Vec3f start, Vec3f end, float amount)
        //{
        //    float num;
        //    Vec3f vector = new Vector3();
        //    if (amount > 1f)
        //    {
        //        num = 1f;
        //    }
        //    else
        //    {
        //        float num2;
        //        if (amount < 0f)
        //        {
        //            num2 = 0f;
        //        }
        //        else
        //        {
        //            num2 = amount;
        //        }
        //        num = num2;
        //    }
        //    double num3 = num;
        //    amount = (float)((3.0 - (num * 2.0)) * (num3 * num3));
        //    vector.X = ((end.X - start.X) * amount) + start.X;
        //    vector.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    vector.Z = ((end.Z - start.Z) * amount) + start.Z;
        //    return vector;
        //}

        //public static float Distance(Vec3f value1, Vec3f value2)
        //{
        //    float x = value1.X - value2.X;
        //    float y = value1.Y - value2.Y;
        //    float z = value1.Z - value2.Z;
        //    double num3 = y;
        //    double num2 = x;
        //    double num = z;
        //    return (float)Math.Sqrt(((num2 * num2) + (num3 * num3)) + (num * num));
        //}

        //public static float DistanceSquared(Vec3f value1, Vec3f value2)
        //{
        //    float x = value1.X - value2.X;
        //    float y = value1.Y - value2.Y;
        //    float z = value1.Z - value2.Z;
        //    double num3 = y;
        //    double num2 = x;
        //    double num = z;
        //    return (float)(((num2 * num2) + (num3 * num3)) + (num * num));
        //}

        //public static float Dot(Vec3f left, Vec3f right)
        //{
        //    return (((left.Y * right.Y) + (left.X * right.X)) + (left.Z * right.Z));
        //}

        //public static void Cross(ref Vec3f left, ref Vec3f right, out Vec3f result)
        //{
        //    Vec3f r = new Vec3f {
        //        X = (left.Y * right.Z) - (left.Z * right.Y),
        //        Y = (left.Z * right.X) - (left.X * right.Z),
        //        Z = (left.X * right.Y) - (left.Y * right.X)
        //    };
        //    result = r;
        //}

        //public static Vec3f Cross(Vec3f left, Vec3f right)
        //{
        //    return new Vec3f { X = (right.Z * left.Y) - (left.Z * right.Y), Y = (left.Z * right.X) - (right.Z * left.X), Z = (right.Y * left.X) - (left.Y * right.X) };
        //}

        //public static void Reflect(ref Vec3f vector, ref Vec3f normal, out Vec3f result)
        //{
        //    float dot = ((vector.Y * normal.Y) + (vector.X * normal.X)) + (vector.Z * normal.Z);
        //    double num = dot * 2.0;
        //    result.X = vector.X - ((float)(normal.X * num));
        //    result.Y = vector.Y - ((float)(normal.Y * num));
        //    result.Z = vector.Z - ((float)(normal.Z * num));
        //}

        //public static Vec3f Reflect(Vec3f vector, Vec3f normal)
        //{
        //    Vec3f result = new Vector3();
        //    float dot = ((vector.Y * normal.Y) + (vector.X * normal.X)) + (vector.Z * normal.Z);
        //    double num = dot * 2.0;
        //    result.X = vector.X - ((float)(normal.X * num));
        //    result.Y = vector.Y - ((float)(normal.Y * num));
        //    result.Z = vector.Z - ((float)(normal.Z * num));
        //    return result;
        //}

        //public static void Transform(ref Vec3f vector, ref Matrix4f transformation, out Vec4f result)
        //{
        //    result = new Vector4();
        //    result.X = (((vector.Y * transformation.M21) + (vector.X * transformation.M11)) + (vector.Z * transformation.M31)) + transformation.M41;
        //    result.Y = (((vector.Y * transformation.M22) + (vector.X * transformation.M12)) + (vector.Z * transformation.M32)) + transformation.M42;
        //    result.Z = (((vector.Y * transformation.M23) + (vector.X * transformation.M13)) + (vector.Z * transformation.M33)) + transformation.M43;
        //    result.W = (((transformation.M24 * vector.Y) + (transformation.M14 * vector.X)) + (vector.Z * transformation.M34)) + transformation.M44;
        //}

        //public static Vec4f Transform(Vec3f vector, Matrix4f transformation)
        //{
        //    return new Vec4f { X = (((transformation.M21 * vector.Y) + (transformation.M11 * vector.X)) + (transformation.M31 * vector.Z)) + transformation.M41, Y = (((transformation.M22 * vector.Y) + (transformation.M12 * vector.X)) + (transformation.M32 * vector.Z)) + transformation.M42, Z = (((transformation.M23 * vector.Y) + (transformation.M13 * vector.X)) + (transformation.M33 * vector.Z)) + transformation.M43, W = (((transformation.M24 * vector.Y) + (transformation.M14 * vector.X)) + (transformation.M34 * vector.Z)) + transformation.M44 };
        //}

        //public static void TransformCoordinate(ref Vec3f coordinate, ref Matrix4f transformation, out Vec3f result)
        //{
        //    Vec3f vector2;
        //    Vec4f vector = new Vec4f {
        //        X = (((coordinate.Y * transformation.M21) + (coordinate.X * transformation.M11)) + (coordinate.Z * transformation.M31)) + transformation.M41,
        //        Y = (((coordinate.Y * transformation.M22) + (coordinate.X * transformation.M12)) + (coordinate.Z * transformation.M32)) + transformation.M42,
        //        Z = (((coordinate.Y * transformation.M23) + (coordinate.X * transformation.M13)) + (coordinate.Z * transformation.M33)) + transformation.M43
        //    };
        //    float num = (float)(1.0 / ((((transformation.M24 * coordinate.Y) + (transformation.M14 * coordinate.X)) + (coordinate.Z * transformation.M34)) + transformation.M44));
        //    vector.W = num;
        //    vector2.X = vector.X * num;
        //    vector2.Y = vector.Y * num;
        //    vector2.Z = vector.Z * num;
        //    result = vector2;
        //}

        public static Vec3f TransformCoordinate(Vec3f coordinate, Matrix4f transformation)
        {
            float num = (1.0f / ((((transformation.M24 * coordinate.Y) + (transformation.M14 * coordinate.X)) + (transformation.M34 * coordinate.Z)) + transformation.M44));
            
            return new Vec3f(
                ((((transformation.M21 * coordinate.Y) + (transformation.M11 * coordinate.X)) + (transformation.M31 * coordinate.Z)) + transformation.M41) * num,
                ((((transformation.M22 * coordinate.Y) + (transformation.M12 * coordinate.X)) + (transformation.M32 * coordinate.Z)) + transformation.M42) * num,
                ((((transformation.M23 * coordinate.Y) + (transformation.M13 * coordinate.X)) + (transformation.M33 * coordinate.Z)) + transformation.M43) * num
            );
        }

        //public static void TransformNormal(ref Vec3f normal, ref Matrix4f transformation, out Vec3f result)
        //{
        //    result.X = ((normal.Y * transformation.M21) + (normal.X * transformation.M11)) + (normal.Z * transformation.M31);
        //    result.Y = ((normal.Y * transformation.M22) + (normal.X * transformation.M12)) + (normal.Z * transformation.M32);
        //    result.Z = ((normal.Y * transformation.M23) + (normal.X * transformation.M13)) + (normal.Z * transformation.M33);
        //}

        //public static Vec3f TransformNormal(Vec3f normal, Matrix4f transformation)
        //{
        //    return new Vec3f { X = ((transformation.M21 * normal.Y) + (transformation.M11 * normal.X)) + (transformation.M31 * normal.Z), Y = ((transformation.M22 * normal.Y) + (transformation.M12 * normal.X)) + (transformation.M32 * normal.Z), Z = ((transformation.M23 * normal.Y) + (transformation.M13 * normal.X)) + (transformation.M33 * normal.Z) };
        //}

        //public static void Project(ref Vec3f vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix4f worldViewProjection, out Vec3f result)
        //{
        //    Vec3f v2 = new Vector3();
        //    Vec3f v = new Vector3();
        //    TransformCoordinate(ref vector, ref worldViewProjection, out v);
        //    v2.X = ((float)(((v.X + 1.0) * 0.5) * width)) + x;
        //    v2.Y = ((float)(((1.0 - v.Y) * 0.5) * height)) + y;
        //    v2.Z = (v.Z * (maxZ - minZ)) + minZ;
        //    result = v2;
        //}

        //public static Vec3f Project(Vec3f vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4f worldViewProjection)
        //{
        //    Vec3f v2;
        //    TransformCoordinate(ref vector, ref worldViewProjection, out vector);
        //    v2.X = ((float)(((vector.X + 1.0) * 0.5) * width)) + x;
        //    v2.Y = ((float)(((1.0 - vector.Y) * 0.5) * height)) + y;
        //    v2.Z = (vector.Z * (maxZ - minZ)) + minZ;
        //    return v2;
        //}

        //public static void Minimize(ref Vec3f value1, ref Vec3f value2, out Vec3f result)
        //{
        //    float z;
        //    float y;
        //    float x;
        //    if (value1.X < value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    result.X = x;
        //    if (value1.Y < value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    result.Y = y;
        //    if (value1.Z < value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    result.Z = z;
        //}

        //public static Vec3f Minimize(Vec3f value1, Vec3f value2)
        //{
        //    float z;
        //    float y;
        //    float x;
        //    Vec3f vector = new Vector3();
        //    if (value1.X < value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    vector.X = x;
        //    if (value1.Y < value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    vector.Y = y;
        //    if (value1.Z < value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    vector.Z = z;
        //    return vector;
        //}

        //public static void Maximize(ref Vec3f value1, ref Vec3f value2, out Vec3f result)
        //{
        //    float z;
        //    float y;
        //    float x;
        //    if (value1.X > value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    result.X = x;
        //    if (value1.Y > value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    result.Y = y;
        //    if (value1.Z > value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    result.Z = z;
        //}

        //public static Vec3f Maximize(Vec3f value1, Vec3f value2)
        //{
        //    float z;
        //    float y;
        //    float x;
        //    Vec3f vector = new Vector3();
        //    if (value1.X > value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    vector.X = x;
        //    if (value1.Y > value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    vector.Y = y;
        //    if (value1.Z > value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    vector.Z = z;
        //    return vector;
        //}


        public static void CrossProduct(Vec3f left, Vec3f right, out Vec3f product)
        {
            product = new Vec3f() {
                X = (left.Y * right.Z) - (left.Z * right.Y),
                Y = (left.Z * right.X) - (left.X * right.Z),
                Z = (left.X * right.Y) - (left.Y * right.X)
            };
        }

        public static Vec3f CrossProduct(Vec3f left, Vec3f right)
        {
            return new Vec3f { X = (left.Y * right.Z) - (left.Z * right.Y), Y = (left.Z * right.X) - (left.X * right.Z), Z = (left.X * right.Y) - (left.Y * right.X) };
        }

        public static float DotProduct(ref Vec3f left, ref Vec3f right)
        {
            return (((left.X * right.X) + (left.Y * right.Y)) + (left.Z * right.Z));
        }

        public static Vec3f TransformCoordinate(ref Vec3f coord, ref Matrix4f transform)
        {
            Vec4f vector;
            vector.X = (((coord.X * transform.M11) + (coord.Y * transform.M21)) + (coord.Z * transform.M31)) + transform.M41;
            vector.Y = (((coord.X * transform.M12) + (coord.Y * transform.M22)) + (coord.Z * transform.M32)) + transform.M42;
            vector.Z = (((coord.X * transform.M13) + (coord.Y * transform.M23)) + (coord.Z * transform.M33)) + transform.M43;
            vector.W = 1f / ((((coord.X * transform.M14) + (coord.Y * transform.M24)) + (coord.Z * transform.M34)) + transform.M44);
            return new Vec3f(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
        }

        public static Vec3f TransformNormal(ref Vec3f coord, Matrix4f tr)
        {
            Vec4f vector;
            Matrix4f matrixx = tr;
            matrixx.M41 = 0f;
            matrixx.M42 = 0f;
            matrixx.M43 = 0f;
            vector.X = (((coord.X * matrixx.M11) + (coord.Y * matrixx.M21)) + (coord.Z * matrixx.M31)) + matrixx.M41;
            vector.Y = (((coord.X * matrixx.M12) + (coord.Y * matrixx.M22)) + (coord.Z * matrixx.M32)) + matrixx.M42;
            vector.Z = (((coord.X * matrixx.M13) + (coord.Y * matrixx.M23)) + (coord.Z * matrixx.M33)) + matrixx.M43;
            vector.W = 1f / ((((coord.X * matrixx.M14) + (coord.Y * matrixx.M24)) + (coord.Z * matrixx.M34)) + matrixx.M44);
            return new Vec3f(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
        }

        public static void Transform(ref Vec3f vector, ref Matrix4f transform, out Vec4f tv)
        {
            tv.X = (((vector.X * transform.M11) + (vector.Y * transform.M21)) + (vector.Z * transform.M31)) + transform.M41;
            tv.Y = (((vector.X * transform.M12) + (vector.Y * transform.M22)) + (vector.Z * transform.M32)) + transform.M42;
            tv.Z = (((vector.X * transform.M13) + (vector.Y * transform.M23)) + (vector.Z * transform.M33)) + transform.M43;
            tv.W = (((vector.X * transform.M14) + (vector.Y * transform.M24)) + (vector.Z * transform.M34)) + transform.M44;
        }


        public static void Transform(float x, float y, float z, ref Matrix4f transform, out Vec4f tv)
        {
            tv.X = (((x * transform.M11) + (y * transform.M21)) + (z * transform.M31)) + transform.M41;
            tv.Y = (((x * transform.M12) + (y * transform.M22)) + (z * transform.M32)) + transform.M42;
            tv.Z = (((x * transform.M13) + (y * transform.M23)) + (z * transform.M33)) + transform.M43;
            tv.W = (((x * transform.M14) + (y * transform.M24)) + (z * transform.M34)) + transform.M44;
        }

        public static void TransformNormal(float x, float y, float z, ref Matrix4f transform, out Vec4f tv)
        {
            tv.X = ((x * transform.M11) + (y * transform.M21)) + (z * transform.M31);
            tv.Y = ((x * transform.M12) + (y * transform.M22)) + (z * transform.M32);
            tv.Z = ((x * transform.M13) + (y * transform.M23)) + (z * transform.M33);
            tv.W = 0f;
        }
    }
}
