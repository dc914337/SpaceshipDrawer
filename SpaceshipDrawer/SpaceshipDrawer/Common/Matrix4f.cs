using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Matrix4f : IEquatable<Matrix4f>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        public float this[int row, int column]
        {
            get
            {
                if (row > 3)
                {
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for Matrix4f run from 0 to 3, inclusive.");
                }
                if (column > 3)
                {
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for Matrix4f run from 0 to 3, inclusive.");
                }
                switch (((row * 4) + column))
                {
                    case 0:
                        return this.M11;

                    case 1:
                        return this.M12;

                    case 2:
                        return this.M13;

                    case 3:
                        return this.M14;

                    case 4:
                        return this.M21;

                    case 5:
                        return this.M22;

                    case 6:
                        return this.M23;

                    case 7:
                        return this.M24;

                    case 8:
                        return this.M31;

                    case 9:
                        return this.M32;

                    case 10:
                        return this.M33;

                    case 11:
                        return this.M34;

                    case 12:
                        return this.M41;

                    case 13:
                        return this.M42;

                    case 14:
                        return this.M43;

                    case 15:
                        return this.M44;
                }
                return 0f;
            }
            set
            {
                if (row > 3)
                {
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for Matrix4f run from 0 to 3, inclusive.");
                }
                if (column > 3)
                {
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for Matrix4f run from 0 to 3, inclusive.");
                }
                switch (((row * 4) + column))
                {
                    case 0:
                        this.M11 = value;
                        break;

                    case 1:
                        this.M12 = value;
                        break;

                    case 2:
                        this.M13 = value;
                        break;

                    case 3:
                        this.M14 = value;
                        break;

                    case 4:
                        this.M21 = value;
                        break;

                    case 5:
                        this.M22 = value;
                        break;

                    case 6:
                        this.M23 = value;
                        break;

                    case 7:
                        this.M24 = value;
                        break;

                    case 8:
                        this.M31 = value;
                        break;

                    case 9:
                        this.M32 = value;
                        break;

                    case 10:
                        this.M33 = value;
                        break;

                    case 11:
                        this.M34 = value;
                        break;

                    case 12:
                        this.M41 = value;
                        break;

                    case 13:
                        this.M42 = value;
                        break;

                    case 14:
                        this.M43 = value;
                        break;

                    case 15:
                        this.M44 = value;
                        break;
                }
            }
        }

        //[Browsable(false)]
        //public Vector4 this[int row]
        //{
        //    get
        //    {
        //        return new Vector4(this[row, 0], this[row, 1], this[row, 2], this[row, 3]);
        //    }
        //    set
        //    {
        //        this[row, 0] = value.X;
        //        this[row, 1] = value.Y;
        //        this[row, 2] = value.Z;
        //        this[row, 3] = value.W;
        //    }
        //}

        //[Browsable(false)]
        //public Vector4 this[int column]
        //{
        //    get
        //    {
        //        return new Vector4(this[0, column], this[1, column], this[2, column], this[3, column]);
        //    }
        //    set
        //    {
        //        this[0, column] = value.X;
        //        this[1, column] = value.Y;
        //        this[2, column] = value.Z;
        //        this[3, column] = value.W;
        //    }
        //}

        public static readonly Matrix4f Identity = new Matrix4f() { M11 = 1f, M22 = 1f, M33 = 1f, M44 = 1f };

        public bool IsIdentity
        {
            get
            {
                if (((this.M11 != 1f) || (this.M22 != 1f)) || ((this.M33 != 1f) || (this.M44 != 1f)))
                {
                    return false;
                }
                return (((((this.M12 == 0f) && (this.M13 == 0f)) && ((this.M14 == 0f) && (this.M21 == 0f))) && (((this.M23 == 0f) && (this.M24 == 0f)) && ((this.M31 == 0f) && (this.M32 == 0f)))) && (((this.M34 == 0f) && (this.M41 == 0f)) && ((this.M42 == 0f) && (this.M43 == 0f))));
            }
        }

        public float[] ToArray()
        {
            return new float[] { this.M11, this.M12, this.M13, this.M14, this.M21, this.M22, this.M23, this.M24, this.M31, this.M32, this.M33, this.M34, this.M41, this.M42, this.M43, this.M44 };
        }

        public float Determinant()
        {
            float temp1 = (this.M44 * this.M33) - (this.M43 * this.M34);
            float temp2 = (this.M32 * this.M44) - (this.M42 * this.M34);
            float temp3 = (this.M32 * this.M43) - (this.M42 * this.M33);
            float temp4 = (this.M31 * this.M44) - (this.M41 * this.M34);
            float temp5 = (this.M31 * this.M43) - (this.M41 * this.M33);
            float temp6 = (this.M31 * this.M42) - (this.M41 * this.M32);
            return (((((((this.M22 * temp1) - (this.M23 * temp2)) + (this.M24 * temp3)) * this.M11) - ((((this.M21 * temp1) - (this.M23 * temp4)) + (this.M24 * temp5)) * this.M12)) + ((((this.M21 * temp2) - (this.M22 * temp4)) + (this.M24 * temp6)) * this.M13)) - ((((this.M21 * temp3) - (this.M22 * temp5)) + (this.M23 * temp6)) * this.M14));
        }

        public static void Add(ref Matrix4f left, ref Matrix4f right, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = left.M11 + right.M11,
                M12 = left.M12 + right.M12,
                M13 = left.M13 + right.M13,
                M14 = left.M14 + right.M14,
                M21 = left.M21 + right.M21,
                M22 = left.M22 + right.M22,
                M23 = left.M23 + right.M23,
                M24 = left.M24 + right.M24,
                M31 = left.M31 + right.M31,
                M32 = left.M32 + right.M32,
                M33 = left.M33 + right.M33,
                M34 = left.M34 + right.M34,
                M41 = left.M41 + right.M41,
                M42 = left.M42 + right.M42,
                M43 = left.M43 + right.M43,
                M44 = left.M44 + right.M44
            };
            result = r;
        }

        public static Matrix4f Add(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = left.M11 + right.M11, M12 = left.M12 + right.M12, M13 = left.M13 + right.M13, M14 = left.M14 + right.M14, M21 = left.M21 + right.M21, M22 = left.M22 + right.M22, M23 = left.M23 + right.M23, M24 = left.M24 + right.M24, M31 = left.M31 + right.M31, M32 = left.M32 + right.M32, M33 = left.M33 + right.M33, M34 = left.M34 + right.M34, M41 = left.M41 + right.M41, M42 = left.M42 + right.M42, M43 = left.M43 + right.M43, M44 = left.M44 + right.M44 };
        }

        public static void Subtract(ref Matrix4f left, ref Matrix4f right, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = left.M11 - right.M11,
                M12 = left.M12 - right.M12,
                M13 = left.M13 - right.M13,
                M14 = left.M14 - right.M14,
                M21 = left.M21 - right.M21,
                M22 = left.M22 - right.M22,
                M23 = left.M23 - right.M23,
                M24 = left.M24 - right.M24,
                M31 = left.M31 - right.M31,
                M32 = left.M32 - right.M32,
                M33 = left.M33 - right.M33,
                M34 = left.M34 - right.M34,
                M41 = left.M41 - right.M41,
                M42 = left.M42 - right.M42,
                M43 = left.M43 - right.M43,
                M44 = left.M44 - right.M44
            };
            result = r;
        }

        public static Matrix4f Subtract(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = left.M11 - right.M11, M12 = left.M12 - right.M12, M13 = left.M13 - right.M13, M14 = left.M14 - right.M14, M21 = left.M21 - right.M21, M22 = left.M22 - right.M22, M23 = left.M23 - right.M23, M24 = left.M24 - right.M24, M31 = left.M31 - right.M31, M32 = left.M32 - right.M32, M33 = left.M33 - right.M33, M34 = left.M34 - right.M34, M41 = left.M41 - right.M41, M42 = left.M42 - right.M42, M43 = left.M43 - right.M43, M44 = left.M44 - right.M44 };
        }

        public static void Multiply(ref Matrix4f left, float right, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = left.M11 * right,
                M12 = left.M12 * right,
                M13 = left.M13 * right,
                M14 = left.M14 * right,
                M21 = left.M21 * right,
                M22 = left.M22 * right,
                M23 = left.M23 * right,
                M24 = left.M24 * right,
                M31 = left.M31 * right,
                M32 = left.M32 * right,
                M33 = left.M33 * right,
                M34 = left.M34 * right,
                M41 = left.M41 * right,
                M42 = left.M42 * right,
                M43 = left.M43 * right,
                M44 = left.M44 * right
            };
            result = r;
        }

        public static Matrix4f Multiply(Matrix4f left, float right)
        {
            return new Matrix4f { M11 = left.M11 * right, M12 = left.M12 * right, M13 = left.M13 * right, M14 = left.M14 * right, M21 = left.M21 * right, M22 = left.M22 * right, M23 = left.M23 * right, M24 = left.M24 * right, M31 = left.M31 * right, M32 = left.M32 * right, M33 = left.M33 * right, M34 = left.M34 * right, M41 = left.M41 * right, M42 = left.M42 * right, M43 = left.M43 * right, M44 = left.M44 * right };
        }

        public static void Multiply(ref Matrix4f left, ref Matrix4f right, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = (((left.M12 * right.M21) + (left.M11 * right.M11)) + (left.M13 * right.M31)) + (left.M14 * right.M41),
                M12 = (((left.M12 * right.M22) + (left.M11 * right.M12)) + (left.M13 * right.M32)) + (left.M14 * right.M42),
                M13 = (((left.M12 * right.M23) + (left.M11 * right.M13)) + (left.M13 * right.M33)) + (left.M14 * right.M43),
                M14 = (((left.M12 * right.M24) + (left.M11 * right.M14)) + (left.M13 * right.M34)) + (left.M14 * right.M44),
                M21 = (((left.M22 * right.M21) + (left.M21 * right.M11)) + (left.M23 * right.M31)) + (left.M24 * right.M41),
                M22 = (((left.M22 * right.M22) + (left.M21 * right.M12)) + (left.M23 * right.M32)) + (left.M24 * right.M42),
                M23 = (((left.M22 * right.M23) + (left.M21 * right.M13)) + (left.M23 * right.M33)) + (left.M24 * right.M43),
                M24 = (((left.M22 * right.M24) + (left.M21 * right.M14)) + (left.M23 * right.M34)) + (left.M24 * right.M44),
                M31 = (((left.M32 * right.M21) + (left.M31 * right.M11)) + (left.M33 * right.M31)) + (left.M34 * right.M41),
                M32 = (((left.M32 * right.M22) + (left.M31 * right.M12)) + (left.M33 * right.M32)) + (left.M34 * right.M42),
                M33 = (((left.M32 * right.M23) + (left.M31 * right.M13)) + (left.M33 * right.M33)) + (left.M34 * right.M43),
                M34 = (((left.M32 * right.M24) + (left.M31 * right.M14)) + (left.M33 * right.M34)) + (left.M34 * right.M44),
                M41 = (((left.M42 * right.M21) + (left.M41 * right.M11)) + (left.M43 * right.M31)) + (left.M44 * right.M41),
                M42 = (((left.M42 * right.M22) + (left.M41 * right.M12)) + (left.M43 * right.M32)) + (left.M44 * right.M42),
                M43 = (((left.M42 * right.M23) + (left.M41 * right.M13)) + (left.M43 * right.M33)) + (left.M44 * right.M43),
                M44 = (((left.M42 * right.M24) + (left.M41 * right.M14)) + (left.M43 * right.M34)) + (left.M44 * right.M44)
            };
            result = r;
        }

        public static Matrix4f Multiply(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = (((right.M21 * left.M12) + (left.M11 * right.M11)) + (right.M31 * left.M13)) + (right.M41 * left.M14), M12 = (((right.M22 * left.M12) + (right.M12 * left.M11)) + (right.M32 * left.M13)) + (right.M42 * left.M14), M13 = (((right.M23 * left.M12) + (right.M13 * left.M11)) + (right.M33 * left.M13)) + (right.M43 * left.M14), M14 = (((right.M24 * left.M12) + (right.M14 * left.M11)) + (right.M34 * left.M13)) + (right.M44 * left.M14), M21 = (((left.M22 * right.M21) + (left.M21 * right.M11)) + (left.M23 * right.M31)) + (left.M24 * right.M41), M22 = (((left.M22 * right.M22) + (left.M21 * right.M12)) + (left.M23 * right.M32)) + (left.M24 * right.M42), M23 = (((right.M23 * left.M22) + (right.M13 * left.M21)) + (right.M33 * left.M23)) + (left.M24 * right.M43), M24 = (((right.M24 * left.M22) + (right.M14 * left.M21)) + (right.M34 * left.M23)) + (right.M44 * left.M24), M31 = (((left.M32 * right.M21) + (left.M31 * right.M11)) + (left.M33 * right.M31)) + (left.M34 * right.M41), M32 = (((left.M32 * right.M22) + (left.M31 * right.M12)) + (left.M33 * right.M32)) + (left.M34 * right.M42), M33 = (((right.M23 * left.M32) + (left.M31 * right.M13)) + (left.M33 * right.M33)) + (left.M34 * right.M43), M34 = (((right.M24 * left.M32) + (right.M14 * left.M31)) + (right.M34 * left.M33)) + (right.M44 * left.M34), M41 = (((left.M42 * right.M21) + (left.M41 * right.M11)) + (left.M43 * right.M31)) + (left.M44 * right.M41), M42 = (((left.M42 * right.M22) + (left.M41 * right.M12)) + (left.M43 * right.M32)) + (left.M44 * right.M42), M43 = (((right.M23 * left.M42) + (left.M41 * right.M13)) + (left.M43 * right.M33)) + (left.M44 * right.M43), M44 = (((right.M24 * left.M42) + (left.M41 * right.M14)) + (right.M34 * left.M43)) + (left.M44 * right.M44) };
        }

        public static void Divide(ref Matrix4f left, float right, out Matrix4f result)
        {
            float inv = (float)(1.0 / ((double)right));
            Matrix4f r = new Matrix4f {
                M11 = left.M11 * inv,
                M12 = left.M12 * inv,
                M13 = left.M13 * inv,
                M14 = left.M14 * inv,
                M21 = left.M21 * inv,
                M22 = left.M22 * inv,
                M23 = left.M23 * inv,
                M24 = left.M24 * inv,
                M31 = left.M31 * inv,
                M32 = left.M32 * inv,
                M33 = left.M33 * inv,
                M34 = left.M34 * inv,
                M41 = left.M41 * inv,
                M42 = left.M42 * inv,
                M43 = left.M43 * inv,
                M44 = left.M44 * inv
            };
            result = r;
        }

        public static Matrix4f Divide(Matrix4f left, float right)
        {
            Matrix4f result = new Matrix4f();
            float inv = (float)(1.0 / ((double)right));
            result.M11 = left.M11 * inv;
            result.M12 = left.M12 * inv;
            result.M13 = left.M13 * inv;
            result.M14 = left.M14 * inv;
            result.M21 = left.M21 * inv;
            result.M22 = left.M22 * inv;
            result.M23 = left.M23 * inv;
            result.M24 = left.M24 * inv;
            result.M31 = left.M31 * inv;
            result.M32 = left.M32 * inv;
            result.M33 = left.M33 * inv;
            result.M34 = left.M34 * inv;
            result.M41 = left.M41 * inv;
            result.M42 = left.M42 * inv;
            result.M43 = left.M43 * inv;
            result.M44 = left.M44 * inv;
            return result;
        }

        public static void Divide(ref Matrix4f left, ref Matrix4f right, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = (float)(((double)left.M11) / ((double)right.M11)),
                M12 = (float)(((double)left.M12) / ((double)right.M12)),
                M13 = (float)(((double)left.M13) / ((double)right.M13)),
                M14 = (float)(((double)left.M14) / ((double)right.M14)),
                M21 = (float)(((double)left.M21) / ((double)right.M21)),
                M22 = (float)(((double)left.M22) / ((double)right.M22)),
                M23 = (float)(((double)left.M23) / ((double)right.M23)),
                M24 = (float)(((double)left.M24) / ((double)right.M24)),
                M31 = (float)(((double)left.M31) / ((double)right.M31)),
                M32 = (float)(((double)left.M32) / ((double)right.M32)),
                M33 = (float)(((double)left.M33) / ((double)right.M33)),
                M34 = (float)(((double)left.M34) / ((double)right.M34)),
                M41 = (float)(((double)left.M41) / ((double)right.M41)),
                M42 = (float)(((double)left.M42) / ((double)right.M42)),
                M43 = (float)(((double)left.M43) / ((double)right.M43)),
                M44 = (float)(((double)left.M44) / ((double)right.M44))
            };
            result = r;
        }

        public static Matrix4f Divide(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = (float)(((double)left.M11) / ((double)right.M11)), M12 = (float)(((double)left.M12) / ((double)right.M12)), M13 = (float)(((double)left.M13) / ((double)right.M13)), M14 = (float)(((double)left.M14) / ((double)right.M14)), M21 = (float)(((double)left.M21) / ((double)right.M21)), M22 = (float)(((double)left.M22) / ((double)right.M22)), M23 = (float)(((double)left.M23) / ((double)right.M23)), M24 = (float)(((double)left.M24) / ((double)right.M24)), M31 = (float)(((double)left.M31) / ((double)right.M31)), M32 = (float)(((double)left.M32) / ((double)right.M32)), M33 = (float)(((double)left.M33) / ((double)right.M33)), M34 = (float)(((double)left.M34) / ((double)right.M34)), M41 = (float)(((double)left.M41) / ((double)right.M41)), M42 = (float)(((double)left.M42) / ((double)right.M42)), M43 = (float)(((double)left.M43) / ((double)right.M43)), M44 = (float)(((double)left.M44) / ((double)right.M44)) };
        }

        public static void Negate(ref Matrix4f matrix, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = -matrix.M11,
                M12 = -matrix.M12,
                M13 = -matrix.M13,
                M14 = -matrix.M14,
                M21 = -matrix.M21,
                M22 = -matrix.M22,
                M23 = -matrix.M23,
                M24 = -matrix.M24,
                M31 = -matrix.M31,
                M32 = -matrix.M32,
                M33 = -matrix.M33,
                M34 = -matrix.M34,
                M41 = -matrix.M41,
                M42 = -matrix.M42,
                M43 = -matrix.M43,
                M44 = -matrix.M44
            };
            result = r;
        }

        public static Matrix4f Negate(Matrix4f matrix)
        {
            return new Matrix4f { M11 = -matrix.M11, M12 = -matrix.M12, M13 = -matrix.M13, M14 = -matrix.M14, M21 = -matrix.M21, M22 = -matrix.M22, M23 = -matrix.M23, M24 = -matrix.M24, M31 = -matrix.M31, M32 = -matrix.M32, M33 = -matrix.M33, M34 = -matrix.M34, M41 = -matrix.M41, M42 = -matrix.M42, M43 = -matrix.M43, M44 = -matrix.M44 };
        }

        //public static void Lerp(ref Matrix4f start, ref Matrix4f end, float amount, out Matrix4f result)
        //{
        //    Matrix4f r = new Matrix4f {
        //        M11 = ((end.M11 - start.M11) * amount) + start.M11,
        //        M12 = ((end.M12 - start.M12) * amount) + start.M12,
        //        M13 = ((end.M13 - start.M13) * amount) + start.M13,
        //        M14 = ((end.M14 - start.M14) * amount) + start.M14,
        //        M21 = ((end.M21 - start.M21) * amount) + start.M21,
        //        M22 = ((end.M22 - start.M22) * amount) + start.M22,
        //        M23 = ((end.M23 - start.M23) * amount) + start.M23,
        //        M24 = ((end.M24 - start.M24) * amount) + start.M24,
        //        M31 = ((end.M31 - start.M31) * amount) + start.M31,
        //        M32 = ((end.M32 - start.M32) * amount) + start.M32,
        //        M33 = ((end.M33 - start.M33) * amount) + start.M33,
        //        M34 = ((end.M34 - start.M34) * amount) + start.M34,
        //        M41 = ((end.M41 - start.M41) * amount) + start.M41,
        //        M42 = ((end.M42 - start.M42) * amount) + start.M42,
        //        M43 = ((end.M43 - start.M43) * amount) + start.M43,
        //        M44 = ((end.M44 - start.M44) * amount) + start.M44
        //    };
        //    result = r;
        //}

        //public static Matrix4f Lerp(Matrix4f start, Matrix4f end, float amount)
        //{
        //    return new Matrix4f { M11 = ((end.M11 - start.M11) * amount) + start.M11, M12 = ((end.M12 - start.M12) * amount) + start.M12, M13 = ((end.M13 - start.M13) * amount) + start.M13, M14 = ((end.M14 - start.M14) * amount) + start.M14, M21 = ((end.M21 - start.M21) * amount) + start.M21, M22 = ((end.M22 - start.M22) * amount) + start.M22, M23 = ((end.M23 - start.M23) * amount) + start.M23, M24 = ((end.M24 - start.M24) * amount) + start.M24, M31 = ((end.M31 - start.M31) * amount) + start.M31, M32 = ((end.M32 - start.M32) * amount) + start.M32, M33 = ((end.M33 - start.M33) * amount) + start.M33, M34 = ((end.M34 - start.M34) * amount) + start.M34, M41 = ((end.M41 - start.M41) * amount) + start.M41, M42 = ((end.M42 - start.M42) * amount) + start.M42, M43 = ((end.M43 - start.M43) * amount) + start.M43, M44 = ((end.M44 - start.M44) * amount) + start.M44 };
        //}

        //public static void Billboard(ref Vec3f objectPosition, ref Vec3f cameraPosition, ref Vec3f cameraUpVector, ref Vec3f cameraForwardVector, out Matrix4f result)
        //{
        //    Vec3f difference = objectPosition - cameraPosition;
        //    Vec3f crossed = new Vec3f();
        //    Vec3f final = new Vec3f();
        //    float lengthSq = difference.LengthSquared();
        //    if (lengthSq < 9.9999997473787516E-05)
        //    {
        //        difference = -cameraForwardVector;
        //    }
        //    else
        //    {
        //        difference = (Vec3f)(difference * ((float)(1.0 / Math.Sqrt((double)lengthSq))));
        //    }
        //    Vec3f.Cross(ref cameraUpVector, ref difference, out crossed);
        //    crossed.Normalize();
        //    Vec3f.Cross(ref difference, ref crossed, out final);
        //    result.M11 = crossed.X;
        //    result.M12 = crossed.Y;
        //    result.M13 = crossed.Z;
        //    result.M14 = 0f;
        //    result.M21 = final.X;
        //    result.M22 = final.Y;
        //    result.M23 = final.Z;
        //    result.M24 = 0f;
        //    result.M31 = difference.X;
        //    result.M32 = difference.Y;
        //    result.M33 = difference.Z;
        //    result.M34 = 0f;
        //    result.M41 = objectPosition.X;
        //    result.M42 = objectPosition.Y;
        //    result.M43 = objectPosition.Z;
        //    result.M44 = 1f;
        //}

        //public static Matrix4f Billboard(Vec3f objectPosition, Vec3f cameraPosition, Vec3f cameraUpVector, Vec3f cameraForwardVector)
        //{
        //    Matrix4f result = new Matrix4f();
        //    Vec3f difference = objectPosition - cameraPosition;
        //    Vec3f crossed = new Vec3f();
        //    Vec3f final = new Vec3f();
        //    float lengthSq = difference.LengthSquared();
        //    if (lengthSq < 9.9999997473787516E-05)
        //    {
        //        difference = -cameraForwardVector;
        //    }
        //    else
        //    {
        //        difference = (Vec3f)(difference * ((float)(1.0 / Math.Sqrt((double)lengthSq))));
        //    }
        //    Vec3f.Cross(ref cameraUpVector, ref difference, out crossed);
        //    crossed.Normalize();
        //    Vec3f.Cross(ref difference, ref crossed, out final);
        //    result.M11 = crossed.X;
        //    result.M12 = crossed.Y;
        //    result.M13 = crossed.Z;
        //    result.M14 = 0f;
        //    result.M21 = final.X;
        //    result.M22 = final.Y;
        //    result.M23 = final.Z;
        //    result.M24 = 0f;
        //    result.M31 = difference.X;
        //    result.M32 = difference.Y;
        //    result.M33 = difference.Z;
        //    result.M34 = 0f;
        //    result.M41 = objectPosition.X;
        //    result.M42 = objectPosition.Y;
        //    result.M43 = objectPosition.Z;
        //    result.M44 = 1f;
        //    return result;
        //}

        public static void RotationX(float angle, out Matrix4f result)
        {
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f RotationX(float angle)
        {
            Matrix4f result = new Matrix4f();
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static void RotationY(float angle, out Matrix4f result)
        {
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = 0f;
            result.M13 = -sin;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = sin;
            result.M32 = 0f;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f RotationY(float angle)
        {
            Matrix4f result = new Matrix4f();
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = 0f;
            result.M13 = -sin;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = sin;
            result.M32 = 0f;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static void RotationZ(float angle, out Matrix4f result)
        {
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f RotationZ(float angle)
        {
            Matrix4f result = new Matrix4f();
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static void RotationAxis(ref Vec3f axis, float angle, out Matrix4f result)
        {
            if (axis.LengthSquared() != 1f)
            {
                axis.Normalize();
            }
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            double num12 = x;
            float xx = (float)(num12 * num12);
            double num11 = y;
            float yy = (float)(num11 * num11);
            double num10 = z;
            float zz = (float)(num10 * num10);
            float xy = y * x;
            float xz = z * x;
            float yz = z * y;
            result.M11 = ((float)((1.0 - xx) * cos)) + xx;
            double num9 = xy;
            double num8 = num9 - (cos * num9);
            double num7 = sin * z;
            result.M12 = (float)(num7 + num8);
            double num6 = xz;
            double num5 = num6 - (cos * num6);
            double num4 = sin * y;
            result.M13 = (float)(num5 - num4);
            result.M14 = 0f;
            result.M21 = (float)(num8 - num7);
            result.M22 = ((float)((1.0 - yy) * cos)) + yy;
            double num3 = yz;
            double num2 = num3 - (cos * num3);
            double num = sin * x;
            result.M23 = (float)(num + num2);
            result.M24 = 0f;
            result.M31 = (float)(num4 + num5);
            result.M32 = (float)(num2 - num);
            result.M33 = ((float)((1.0 - zz) * cos)) + zz;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f RotationAxis(Vec3f axis, float angle)
        {
            if (axis.LengthSquared() != 1f)
            {
                axis.Normalize();
            }
            Matrix4f result = new Matrix4f();
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float cos = (float)Math.Cos((double)angle);
            float sin = (float)Math.Sin((double)angle);
            double num12 = x;
            float xx = (float)(num12 * num12);
            double num11 = y;
            float yy = (float)(num11 * num11);
            double num10 = z;
            float zz = (float)(num10 * num10);
            float xy = y * x;
            float xz = z * x;
            float yz = z * y;
            result.M11 = ((float)((1.0 - xx) * cos)) + xx;
            double num9 = xy;
            double num8 = num9 - (cos * num9);
            double num7 = sin * z;
            result.M12 = (float)(num7 + num8);
            double num6 = xz;
            double num5 = num6 - (cos * num6);
            double num4 = sin * y;
            result.M13 = (float)(num5 - num4);
            result.M14 = 0f;
            result.M21 = (float)(num8 - num7);
            result.M22 = ((float)((1.0 - yy) * cos)) + yy;
            double num3 = yz;
            double num2 = num3 - (cos * num3);
            double num = sin * x;
            result.M23 = (float)(num + num2);
            result.M24 = 0f;
            result.M31 = (float)(num4 + num5);
            result.M32 = (float)(num2 - num);
            result.M33 = ((float)((1.0 - zz) * cos)) + zz;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static void Scaling(ref Vec3f scale, out Matrix4f result)
        {
            result.M11 = scale.X;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = scale.Y;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scale.Z;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f Scaling(Vec3f scale)
        {
            return new Matrix4f { M11 = scale.X, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = scale.Y, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = scale.Z, M34 = 0f, M41 = 0f, M42 = 0f, M43 = 0f, M44 = 1f };
        }

        public static void Scaling(float x, float y, float z, out Matrix4f result)
        {
            result.M11 = x;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = y;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = z;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix4f Scaling(float x, float y, float z)
        {
            return new Matrix4f { M11 = x, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = y, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = z, M34 = 0f, M41 = 0f, M42 = 0f, M43 = 0f, M44 = 1f };
        }

        public static void Translation(ref Vec3f amount, out Matrix4f result)
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = amount.X;
            result.M42 = amount.Y;
            result.M43 = amount.Z;
            result.M44 = 1f;
        }

        public static Matrix4f Translation(Vec3f amount)
        {
            return new Matrix4f { M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = 1f, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = 1f, M34 = 0f, M41 = amount.X, M42 = amount.Y, M43 = amount.Z, M44 = 1f };
        }

        public static void Translation(float x, float y, float z, out Matrix4f result)
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1f;
        }

        public static Matrix4f Translation(float x, float y, float z)
        {
            return new Matrix4f { M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = 1f, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = 1f, M34 = 0f, M41 = x, M42 = y, M43 = z, M44 = 1f };
        }

        public static void Transpose(ref Matrix4f matrix, out Matrix4f result)
        {
            Matrix4f r = new Matrix4f {
                M11 = matrix.M11,
                M12 = matrix.M21,
                M13 = matrix.M31,
                M14 = matrix.M41,
                M21 = matrix.M12,
                M22 = matrix.M22,
                M23 = matrix.M32,
                M24 = matrix.M42,
                M31 = matrix.M13,
                M32 = matrix.M23,
                M33 = matrix.M33,
                M34 = matrix.M43,
                M41 = matrix.M14,
                M42 = matrix.M24,
                M43 = matrix.M34,
                M44 = matrix.M44
            };
            result = r;
        }

        public static Matrix4f Transpose(Matrix4f matrix)
        {
            return new Matrix4f { M11 = matrix.M11, M12 = matrix.M21, M13 = matrix.M31, M14 = matrix.M41, M21 = matrix.M12, M22 = matrix.M22, M23 = matrix.M32, M24 = matrix.M42, M31 = matrix.M13, M32 = matrix.M23, M33 = matrix.M33, M34 = matrix.M43, M41 = matrix.M14, M42 = matrix.M24, M43 = matrix.M34, M44 = matrix.M44 };
        }

        public static Matrix4f operator -(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = left.M11 - right.M11, M12 = left.M12 - right.M12, M13 = left.M13 - right.M13, M14 = left.M14 - right.M14, M21 = left.M21 - right.M21, M22 = left.M22 - right.M22, M23 = left.M23 - right.M23, M24 = left.M24 - right.M24, M31 = left.M31 - right.M31, M32 = left.M32 - right.M32, M33 = left.M33 - right.M33, M34 = left.M34 - right.M34, M41 = left.M41 - right.M41, M42 = left.M42 - right.M42, M43 = left.M43 - right.M43, M44 = left.M44 - right.M44 };
        }

        public static Matrix4f operator -(Matrix4f matrix)
        {
            return new Matrix4f { M11 = -matrix.M11, M12 = -matrix.M12, M13 = -matrix.M13, M14 = -matrix.M14, M21 = -matrix.M21, M22 = -matrix.M22, M23 = -matrix.M23, M24 = -matrix.M24, M31 = -matrix.M31, M32 = -matrix.M32, M33 = -matrix.M33, M34 = -matrix.M34, M41 = -matrix.M41, M42 = -matrix.M42, M43 = -matrix.M43, M44 = -matrix.M44 };
        }

        public static Matrix4f operator +(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = left.M11 + right.M11, M12 = left.M12 + right.M12, M13 = left.M13 + right.M13, M14 = left.M14 + right.M14, M21 = left.M21 + right.M21, M22 = left.M22 + right.M22, M23 = left.M23 + right.M23, M24 = left.M24 + right.M24, M31 = left.M31 + right.M31, M32 = left.M32 + right.M32, M33 = left.M33 + right.M33, M34 = left.M34 + right.M34, M41 = left.M41 + right.M41, M42 = left.M42 + right.M42, M43 = left.M43 + right.M43, M44 = left.M44 + right.M44 };
        }

        public static Matrix4f operator /(Matrix4f left, float right)
        {
            return new Matrix4f { M11 = (float)(((double)left.M11) / ((double)right)), M12 = (float)(((double)left.M12) / ((double)right)), M13 = (float)(((double)left.M13) / ((double)right)), M14 = (float)(((double)left.M14) / ((double)right)), M21 = (float)(((double)left.M21) / ((double)right)), M22 = (float)(((double)left.M22) / ((double)right)), M23 = (float)(((double)left.M23) / ((double)right)), M24 = (float)(((double)left.M24) / ((double)right)), M31 = (float)(((double)left.M31) / ((double)right)), M32 = (float)(((double)left.M32) / ((double)right)), M33 = (float)(((double)left.M33) / ((double)right)), M34 = (float)(((double)left.M34) / ((double)right)), M41 = (float)(((double)left.M41) / ((double)right)), M42 = (float)(((double)left.M42) / ((double)right)), M43 = (float)(((double)left.M43) / ((double)right)), M44 = (float)(((double)left.M44) / ((double)right)) };
        }

        public static Matrix4f operator /(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = (float)(((double)left.M11) / ((double)right.M11)), M12 = (float)(((double)left.M12) / ((double)right.M12)), M13 = (float)(((double)left.M13) / ((double)right.M13)), M14 = (float)(((double)left.M14) / ((double)right.M14)), M21 = (float)(((double)left.M21) / ((double)right.M21)), M22 = (float)(((double)left.M22) / ((double)right.M22)), M23 = (float)(((double)left.M23) / ((double)right.M23)), M24 = (float)(((double)left.M24) / ((double)right.M24)), M31 = (float)(((double)left.M31) / ((double)right.M31)), M32 = (float)(((double)left.M32) / ((double)right.M32)), M33 = (float)(((double)left.M33) / ((double)right.M33)), M34 = (float)(((double)left.M34) / ((double)right.M34)), M41 = (float)(((double)left.M41) / ((double)right.M41)), M42 = (float)(((double)left.M42) / ((double)right.M42)), M43 = (float)(((double)left.M43) / ((double)right.M43)), M44 = (float)(((double)left.M44) / ((double)right.M44)) };
        }

        public static Matrix4f operator *(float left, Matrix4f right)
        {
            return (Matrix4f)(right * left);
        }

        public static Matrix4f operator *(Matrix4f left, float right)
        {
            return new Matrix4f { M11 = left.M11 * right, M12 = left.M12 * right, M13 = left.M13 * right, M14 = left.M14 * right, M21 = left.M21 * right, M22 = left.M22 * right, M23 = left.M23 * right, M24 = left.M24 * right, M31 = left.M31 * right, M32 = left.M32 * right, M33 = left.M33 * right, M34 = left.M34 * right, M41 = left.M41 * right, M42 = left.M42 * right, M43 = left.M43 * right, M44 = left.M44 * right };
        }

        public static Matrix4f operator *(Matrix4f left, Matrix4f right)
        {
            return new Matrix4f { M11 = (((right.M21 * left.M12) + (left.M11 * right.M11)) + (right.M31 * left.M13)) + (right.M41 * left.M14), M12 = (((right.M22 * left.M12) + (right.M12 * left.M11)) + (right.M32 * left.M13)) + (right.M42 * left.M14), M13 = (((right.M23 * left.M12) + (right.M13 * left.M11)) + (right.M33 * left.M13)) + (right.M43 * left.M14), M14 = (((right.M24 * left.M12) + (right.M14 * left.M11)) + (right.M34 * left.M13)) + (right.M44 * left.M14), M21 = (((left.M22 * right.M21) + (left.M21 * right.M11)) + (left.M23 * right.M31)) + (left.M24 * right.M41), M22 = (((left.M22 * right.M22) + (left.M21 * right.M12)) + (left.M23 * right.M32)) + (left.M24 * right.M42), M23 = (((right.M23 * left.M22) + (right.M13 * left.M21)) + (right.M33 * left.M23)) + (left.M24 * right.M43), M24 = (((right.M24 * left.M22) + (right.M14 * left.M21)) + (right.M34 * left.M23)) + (right.M44 * left.M24), M31 = (((left.M32 * right.M21) + (left.M31 * right.M11)) + (left.M33 * right.M31)) + (left.M34 * right.M41), M32 = (((left.M32 * right.M22) + (left.M31 * right.M12)) + (left.M33 * right.M32)) + (left.M34 * right.M42), M33 = (((right.M23 * left.M32) + (left.M31 * right.M13)) + (left.M33 * right.M33)) + (left.M34 * right.M43), M34 = (((right.M24 * left.M32) + (right.M14 * left.M31)) + (right.M34 * left.M33)) + (right.M44 * left.M34), M41 = (((left.M42 * right.M21) + (left.M41 * right.M11)) + (left.M43 * right.M31)) + (left.M44 * right.M41), M42 = (((left.M42 * right.M22) + (left.M41 * right.M12)) + (left.M43 * right.M32)) + (left.M44 * right.M42), M43 = (((right.M23 * left.M42) + (left.M41 * right.M13)) + (left.M43 * right.M33)) + (left.M44 * right.M43), M44 = (((right.M24 * left.M42) + (left.M41 * right.M14)) + (right.M34 * left.M43)) + (left.M44 * right.M44) };
        }

        public static bool operator ==(Matrix4f left, Matrix4f right)
        {
            return Equals(ref left, ref right);
        }

        public static bool operator !=(Matrix4f left, Matrix4f right)
        {
            return !Equals(ref left, ref right);
        }

        public override string ToString()
        {
            return string.Format(
                "[[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]]",
                this.M11, this.M12, this.M13, this.M14, this.M21, this.M22, this.M23, this.M24, this.M31, this.M32, this.M33, this.M34, this.M41, this.M42, this.M43, this.M44
            );
        }

        public override int GetHashCode()
        {
            float num17 = this.M11;
            float num16 = this.M12;
            float num15 = this.M13;
            float num14 = this.M14;
            float num13 = this.M21;
            float num12 = this.M22;
            float num11 = this.M23;
            float num10 = this.M24;
            float num9 = this.M31;
            float num8 = this.M32;
            float num7 = this.M33;
            float num6 = this.M34;
            float num5 = this.M41;
            float num4 = this.M42;
            float num3 = this.M43;
            float num2 = this.M44;
            int num = (((((((((((((num3.GetHashCode() + num2.GetHashCode()) + num4.GetHashCode()) + num5.GetHashCode()) + num6.GetHashCode()) + num7.GetHashCode()) + num8.GetHashCode()) + num9.GetHashCode()) + num10.GetHashCode()) + num11.GetHashCode()) + num12.GetHashCode()) + num13.GetHashCode()) + num14.GetHashCode()) + num15.GetHashCode()) + num16.GetHashCode();
            return (num17.GetHashCode() + num);
        }

        public static bool Equals(ref Matrix4f value1, ref Matrix4f value2)
        {
            return (((((value1.M11 == value2.M11) && (value1.M12 == value2.M12)) && ((value1.M13 == value2.M13) && (value1.M14 == value2.M14))) && (((value1.M21 == value2.M21) && (value1.M22 == value2.M22)) && ((value1.M23 == value2.M23) && (value1.M24 == value2.M24)))) && ((((value1.M31 == value2.M31) && (value1.M32 == value2.M32)) && ((value1.M33 == value2.M33) && (value1.M34 == value2.M34))) && (((value1.M41 == value2.M41) && (value1.M42 == value2.M42)) && ((value1.M43 == value2.M43) && (value1.M44 == value2.M44)))));
        }

        public bool Equals(Matrix4f other)
        {
            return (((((this.M11 == other.M11) && (this.M12 == other.M12)) && ((this.M13 == other.M13) && (this.M14 == other.M14))) && (((this.M21 == other.M21) && (this.M22 == other.M22)) && ((this.M23 == other.M23) && (this.M24 == other.M24)))) && ((((this.M31 == other.M31) && (this.M32 == other.M32)) && ((this.M33 == other.M33) && (this.M34 == other.M34))) && (((this.M41 == other.M41) && (this.M42 == other.M42)) && ((this.M43 == other.M43) && (this.M44 == other.M44)))));
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
            return this.Equals((Matrix4f)obj);
        }

        public static Matrix4f RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            return RotationQuaternion(Quaternion4f.RotationYawPitchRoll(yaw, pitch, roll));
        }

        public static Matrix4f RotationQuaternion(Quaternion4f rotation)
        {
            float x = rotation.X;
            double num7 = x;
            float xx = (float)(num7 * num7);
            float y = rotation.Y;
            double num6 = y;
            float yy = (float)(num6 * num6);
            float z = rotation.Z;
            double num5 = z;
            float zz = (float)(num5 * num5);
            float xy = y * x;
            float w = rotation.W;
            float zw = w * z;
            float zx = z * x;
            float yw = w * y;
            float yz = z * y;
            float xw = w * x;

            return new Matrix4f() {
                M11 = (float)(1.0 - ((zz + yy) * 2.0)),
                M12 = (float)((zw + xy) * 2.0),
                M13 = (float)((zx - yw) * 2.0),
                M14 = 0f,
                M21 = (float)((xy - zw) * 2.0),
                M22 = (float)(1.0 - ((zz + xx) * 2.0)),
                M23 = (float)((xw + yz) * 2.0),
                M24 = 0f,
                M31 = (float)((yw + zx) * 2.0),
                M32 = (float)((yz - xw) * 2.0),
                M33 = (float)(1.0 - ((yy + xx) * 2.0)),
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f,
            }; ;
        }
    }
}
