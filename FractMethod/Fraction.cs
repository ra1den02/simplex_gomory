
using System;

namespace Mehroz
{
  public class Fraction
  {
    private long m_iNumerator;
    private long m_iDenominator;

    public Fraction() => this.Initialize(0L, 1L);

    public Fraction(long iWholeNumber) => this.Initialize(iWholeNumber, 1L);

    public Fraction(double dDecimalValue)
    {
      Fraction fraction = Fraction.ToFraction(dDecimalValue);
      this.Initialize(fraction.Numerator, fraction.Denominator);
    }

    public Fraction(string strValue)
    {
      Fraction fraction = Fraction.ToFraction(strValue);
      this.Initialize(fraction.Numerator, fraction.Denominator);
    }

    public Fraction(long iNumerator, long iDenominator) => this.Initialize(iNumerator, iDenominator);

    private void Initialize(long iNumerator, long iDenominator)
    {
      this.Numerator = iNumerator;
      this.Denominator = iDenominator;
      Fraction.ReduceFraction(this);
    }

    public long Denominator
    {
      get => this.m_iDenominator;
      set => this.m_iDenominator = value != 0L ? value : throw new FractionException("Denominator cannot be assigned a ZERO Value");
    }

    public long Numerator
    {
      get => this.m_iNumerator;
      set => this.m_iNumerator = value;
    }

    public long Value
    {
      set
      {
        this.m_iNumerator = value;
        this.m_iDenominator = 1L;
      }
    }

    public double ToDouble() => (double) this.Numerator / (double) this.Denominator;

    public override string ToString() => this.Denominator != 1L ? this.Numerator.ToString() + "/" + (object) this.Denominator : this.Numerator.ToString();

    public static Fraction ToFraction(string strValue)
    {
      int num = 0;
      while (num < strValue.Length && strValue[num] != '/')
        ++num;
      return num == strValue.Length ? (Fraction) Convert.ToDouble(strValue) : new Fraction(Convert.ToInt64(strValue.Substring(0, num)), Convert.ToInt64(strValue.Substring(num + 1)));
    }

    public static Fraction ToFraction(double dValue)
    {
      try
      {
        Fraction fraction;
        if (dValue % 1.0 == 0.0)
        {
          fraction = new Fraction(checked ((long) dValue));
        }
        else
        {
          double a = dValue;
          long iDenominator = 1;
          string str1;
          for (str1 = dValue.ToString(); str1.IndexOf("E") > 0; str1 = a.ToString())
          {
            a *= 10.0;
            checked { iDenominator *= 10L; }
          }
          int index = 0;
          string str2 = str1.Replace(",", ".");
          while (str2[index] != '.')
            checked { ++index; }
          int num = checked (str2.Length - index - 1);
          while (num > 0)
          {
            a *= 10.0;
            checked { iDenominator *= 10L; }
            checked { --num; }
          }
          fraction = new Fraction((long) checked ((int) Math.Round(a)), iDenominator);
        }
        return fraction;
      }
      catch (OverflowException ex)
      {
        throw new FractionException("Conversion not possible due to overflow");
      }
      catch (Exception ex)
      {
        throw new FractionException("Conversion not possible");
      }
    }

    public Fraction Duplicate() => new Fraction()
    {
      Numerator = this.Numerator,
      Denominator = this.Denominator
    };

    public static Fraction Inverse(Fraction frac1)
    {
      if (frac1.Numerator == 0L)
        throw new FractionException("Operation not possible (Denominator cannot be assigned a ZERO Value)");
      return new Fraction(frac1.Denominator, frac1.Numerator);
    }

    public static Fraction operator -(Fraction frac1) => Fraction.Negate(frac1);

    public static Fraction operator +(Fraction frac1, Fraction frac2) => Fraction.Add(frac1, frac2);

    public static Fraction operator +(int iNo, Fraction frac1) => Fraction.Add(frac1, new Fraction((long) iNo));

    public static Fraction operator +(Fraction frac1, int iNo) => Fraction.Add(frac1, new Fraction((long) iNo));

    public static Fraction operator +(double dbl, Fraction frac1) => Fraction.Add(frac1, Fraction.ToFraction(dbl));

    public static Fraction operator +(Fraction frac1, double dbl) => Fraction.Add(frac1, Fraction.ToFraction(dbl));

    public static Fraction operator -(Fraction frac1, Fraction frac2) => Fraction.Add(frac1, -frac2);

    public static Fraction operator -(int iNo, Fraction frac1) => Fraction.Add(-frac1, new Fraction((long) iNo));

    public static Fraction operator -(Fraction frac1, int iNo) => Fraction.Add(frac1, -new Fraction((long) iNo));

    public static Fraction operator -(double dbl, Fraction frac1) => Fraction.Add(-frac1, Fraction.ToFraction(dbl));

    public static Fraction operator -(Fraction frac1, double dbl) => Fraction.Add(frac1, -Fraction.ToFraction(dbl));

    public static Fraction operator *(Fraction frac1, Fraction frac2) => Fraction.Multiply(frac1, frac2);

    public static Fraction operator *(int iNo, Fraction frac1) => Fraction.Multiply(frac1, new Fraction((long) iNo));

    public static Fraction operator *(Fraction frac1, int iNo) => Fraction.Multiply(frac1, new Fraction((long) iNo));

    public static Fraction operator *(double dbl, Fraction frac1) => Fraction.Multiply(frac1, Fraction.ToFraction(dbl));

    public static Fraction operator *(Fraction frac1, double dbl) => Fraction.Multiply(frac1, Fraction.ToFraction(dbl));

    public static Fraction operator /(Fraction frac1, Fraction frac2) => Fraction.Multiply(frac1, Fraction.Inverse(frac2));

    public static Fraction operator /(int iNo, Fraction frac1) => Fraction.Multiply(Fraction.Inverse(frac1), new Fraction((long) iNo));

    public static Fraction operator /(Fraction frac1, int iNo) => Fraction.Multiply(frac1, Fraction.Inverse(new Fraction((long) iNo)));

    public static Fraction operator /(double dbl, Fraction frac1) => Fraction.Multiply(Fraction.Inverse(frac1), Fraction.ToFraction(dbl));

    public static Fraction operator /(Fraction frac1, double dbl) => Fraction.Multiply(frac1, Fraction.Inverse(Fraction.ToFraction(dbl)));

    public static bool operator ==(Fraction frac1, Fraction frac2) => frac1.Equals((object) frac2);

    public static bool operator !=(Fraction frac1, Fraction frac2) => !frac1.Equals((object) frac2);

    public static bool operator ==(Fraction frac1, int iNo) => frac1.Equals((object) new Fraction((long) iNo));

    public static bool operator !=(Fraction frac1, int iNo) => !frac1.Equals((object) new Fraction((long) iNo));

    public static bool operator ==(Fraction frac1, double dbl) => frac1.Equals((object) new Fraction(dbl));

    public static bool operator !=(Fraction frac1, double dbl) => !frac1.Equals((object) new Fraction(dbl));

    public static bool operator <(Fraction frac1, Fraction frac2) => frac1.Numerator * frac2.Denominator < frac2.Numerator * frac1.Denominator;

    public static bool operator >(Fraction frac1, Fraction frac2) => frac1.Numerator * frac2.Denominator > frac2.Numerator * frac1.Denominator;

    public static bool operator <=(Fraction frac1, Fraction frac2) => frac1.Numerator * frac2.Denominator <= frac2.Numerator * frac1.Denominator;

    public static bool operator >=(Fraction frac1, Fraction frac2) => frac1.Numerator * frac2.Denominator >= frac2.Numerator * frac1.Denominator;

    public static implicit operator Fraction(long lNo) => new Fraction(lNo);

    public static implicit operator Fraction(double dNo) => new Fraction(dNo);

    public static implicit operator Fraction(string strNo) => new Fraction(strNo);

    public static explicit operator double(Fraction frac) => frac.ToDouble();

    public static implicit operator string(Fraction frac) => frac.ToString();

    public override bool Equals(object obj)
    {
      Fraction fraction = (Fraction) obj;
      return this.Numerator == fraction.Numerator && this.Denominator == fraction.Denominator;
    }

    public override int GetHashCode() => Convert.ToInt32((this.Numerator ^ this.Denominator) & (long) uint.MaxValue);

    private static Fraction Negate(Fraction frac1) => new Fraction(-frac1.Numerator, frac1.Denominator);

    private static Fraction Add(Fraction frac1, Fraction frac2)
    {
      try
      {
        return new Fraction(checked (frac1.Numerator * frac2.Denominator + frac2.Numerator * frac1.Denominator), checked (frac1.Denominator * frac2.Denominator));
      }
      catch (OverflowException ex)
      {
        throw new FractionException("Overflow occurred while performing arithemetic operation");
      }
      catch (Exception ex)
      {
        throw new FractionException("An error occurred while performing arithemetic operation");
      }
    }

    private static Fraction Multiply(Fraction frac1, Fraction frac2)
    {
      try
      {
        return new Fraction(checked (frac1.Numerator * frac2.Numerator), checked (frac1.Denominator * frac2.Denominator));
      }
      catch (OverflowException ex)
      {
        throw new FractionException("Overflow occurred while performing arithemetic operation");
      }
      catch (Exception ex)
      {
        throw new FractionException("An error occurred while performing arithemetic operation");
      }
    }

    private static long GCD(long iNo1, long iNo2)
    {
      if (iNo1 < 0L)
        iNo1 = -iNo1;
      if (iNo2 < 0L)
        iNo2 = -iNo2;
      do
      {
        if (iNo1 < iNo2)
        {
          long num = iNo1;
          iNo1 = iNo2;
          iNo2 = num;
        }
        iNo1 %= iNo2;
      }
      while (iNo1 != 0L);
      return iNo2;
    }

    public static void ReduceFraction(Fraction frac)
    {
      try
      {
        if (frac.Numerator == 0L)
        {
          frac.Denominator = 1L;
        }
        else
        {
          long num = Fraction.GCD(frac.Numerator, frac.Denominator);
          frac.Numerator /= num;
          frac.Denominator /= num;
          if (frac.Denominator < 0L)
          {
            frac.Numerator *= -1L;
            frac.Denominator *= -1L;
          }
        }
      }
      catch (Exception ex)
      {
        throw new FractionException("Cannot reduce Fraction: " + ex.Message);
      }
    }
  }
}
