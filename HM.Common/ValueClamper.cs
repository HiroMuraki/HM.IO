using System.Numerics;

namespace HM.Common;

public static class ValueClamper
{
    public static T Clamp<T>(T value, T minValue, T maxValue)
        where T : IComparisonOperators<T, T, Boolean>
    {
        if (maxValue < minValue)
        {
            throw new ArgumentException($"Max value can't less than Min value");
        }

        if (value < minValue)
        {
            return minValue;
        }
        if (value > maxValue)
        {
            return maxValue;
        }

        return value;
    }
}