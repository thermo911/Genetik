using Genetik.Core;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Crossing;

public class RandomSinglePointCrosser : ICrosser<Vec2>
{
    private Random _random = new Random(Guid.NewGuid().GetHashCode());
    public Tuple<Genome<Vec2>, Genome<Vec2>> CrossGenomes(Genome<Vec2> genome1, Genome<Vec2> genome2)
    {
        if (genome1.Length != genome2.Length)
            throw new ArgumentException("genomes lengths differ");

        int prefixLength = _random.Next(1, genome1.Length);
        var genes1 = new Vec2[genome1.Length];
        var genes2 = new Vec2[genome1.Length];

        int pos = 0;
        while (pos < prefixLength)
        {
            genes1[pos] = genome1.Genes[pos];
            genes2[pos] = genome2.Genes[pos];
            pos++;
        }

        while (pos < genes1.Length)
        {
            genes1[pos] = genome2.Genes[pos];
            genes2[pos] = genome1.Genes[pos];
            pos++;
        }

        return new Tuple<Genome<Vec2>, Genome<Vec2>>(
            new Genome<Vec2>(genes1),
            new Genome<Vec2>(genes2));
    }
}