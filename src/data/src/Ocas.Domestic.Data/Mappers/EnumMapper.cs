using System;

namespace Ocas.Domestic.Data.Mappers
{
    /// <summary>
    /// https://codereview.stackexchange.com/a/201814
    /// </summary>
    internal static class EnumMapper
    {
        public static TEnum ToEnum<TEnum>(this byte value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this sbyte value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this ushort value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this short value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this uint value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this int value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this ulong value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        public static TEnum ToEnum<TEnum>(this long value)
            where TEnum : struct
        {
            return DynamicToEnum<TEnum>(value);
        }

        private static TEnum DynamicToEnum<TEnum>(dynamic value)
            where TEnum : struct
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type} is not an enum.");
            }

            if (type.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
            {
                var values = Enum.GetValues(type);

                void testEnum<T>(T myVal, Func<T, bool> aboveZero, Func<T, T, T> and, Func<T, T, T> xor)
                {
                    foreach (T val in values)
                    {
                        if (aboveZero(and(myVal, val)))
                        {
                            myVal = xor(myVal, val);
                        }
                    }

                    if (aboveZero(myVal))
                    {
                        throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                }

                var underlyingType = Enum.GetUnderlyingType(type).FullName;
                switch (underlyingType)
                {
                    case "System.Byte":
                        testEnum((byte)value, (v) => v > (byte)0, (v1, v2) => (byte)(v1 & v2), (v1, v2) => (byte)(v1 ^ v2));
                        break;
                    case "System.SByte":
                        testEnum((sbyte)value, (v) => v > (sbyte)0, (v1, v2) => (sbyte)(v1 & v2), (v1, v2) => (sbyte)(v1 ^ v2));
                        break;
                    case "System.UInt16":
                        testEnum((ushort)value, (v) => v > (ushort)0, (v1, v2) => (ushort)(v1 & v2), (v1, v2) => (ushort)(v1 ^ v2));
                        break;
                    case "System.Int16":
                        testEnum((short)value, (v) => v > (short)0, (v1, v2) => (short)(v1 & v2), (v1, v2) => (short)(v1 ^ v2));
                        break;
                    case "System.UInt32":
                        testEnum((uint)value, (v) => v > (uint)0, (v1, v2) => (uint)(v1 & v2), (v1, v2) => (uint)(v1 ^ v2));
                        break;
                    case "System.Int32":
                        testEnum((int)value, (v) => v > (int)0, (v1, v2) => (int)(v1 & v2), (v1, v2) => (int)(v1 ^ v2));
                        break;
                    case "System.UInt64":
                        testEnum((ulong)value, (v) => v > (ulong)0, (v1, v2) => (ulong)(v1 & v2), (v1, v2) => (ulong)(v1 ^ v2));
                        break;
                    case "System.Int64":
                        testEnum((long)value, (v) => v > (long)0, (v1, v2) => (long)(v1 & v2), (v1, v2) => (long)(v1 ^ v2));
                        break;
                    default:
                        throw new ArgumentException($"{type} does not have a valid backing type ({underlyingType}).");
                }
            }
            else if (!type.IsEnumDefined(value))
            {
                throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
            }

            return (TEnum)Enum.ToObject(type, value);
        }
    }
}