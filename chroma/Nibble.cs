//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.Numerics;

//namespace Chroma;

///// <summary>
///// Represents a 4-bit number.
///// </summary>
//public readonly struct Nibble : INumberBase<Nibble>, ISpanParsable<Nibble>
//{
//    public static Nibble One { get; }
//    public static int Radix { get; }
//    public static Nibble Zero { get; }
//    public static Nibble AdditiveIdentity { get; }
//    public static Nibble MultiplicativeIdentity { get; }

//    public static Nibble Abs(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsCanonical(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsComplexNumber(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsEvenInteger(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsFinite(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsImaginaryNumber(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsInfinity(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsInteger(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsNaN(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsNegative(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsNegativeInfinity(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsNormal(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsOddInteger(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsPositive(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsPositiveInfinity(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsRealNumber(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsSubnormal(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool IsZero(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble MaxMagnitude(Nibble x, Nibble y)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble MaxMagnitudeNumber(Nibble x, Nibble y)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble MinMagnitude(Nibble x, Nibble y)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble MinMagnitudeNumber(Nibble x, Nibble y)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble Parse(string s, NumberStyles style, IFormatProvider? provider)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble Parse(string s, IFormatProvider? provider)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Nibble result)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Nibble result)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Nibble result)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Nibble result)
//    {
//        throw new NotImplementedException();
//    }

//    public bool Equals(Nibble other)
//    {
//        throw new NotImplementedException();
//    }

//    public string ToString(string? format, IFormatProvider? formatProvider)
//    {
//        throw new NotImplementedException();
//    }

//    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator +(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator +(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator -(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator -(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator ++(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator --(Nibble value)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator *(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }

//    public static Nibble operator /(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool operator ==(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }

//    public static bool operator !=(Nibble left, Nibble right)
//    {
//        throw new NotImplementedException();
//    }
//}