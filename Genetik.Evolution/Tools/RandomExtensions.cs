namespace Genetik.Evolution.Tools;

public static class RandomExtensions
{
    public static Tuple<int, int> NextDistinctPair(this Random random, int maxValue)
    {
        if (maxValue < 2)
            throw new ArgumentOutOfRangeException(nameof(maxValue), $"{nameof(maxValue)} < 2");

        int value1 = random.Next(maxValue);
        int value2 = (value1 + random.Next(maxValue - 1) + 1) % maxValue;

        return new Tuple<int, int>(value1, value2);
    }

    public static Tuple<int, int> NextDistinctPair(this Random random, int minValue, int maxValue)
    {
        if (maxValue - minValue < 2)
        {
            throw new ArgumentException(
                $"{nameof(maxValue)} - {nameof(minValue)} < 2");
        }

        int diff = maxValue - minValue;
        var pair = random.NextDistinctPair(diff);

        return new Tuple<int, int>(pair.Item1 + minValue, pair.Item2 + minValue);
    }
}