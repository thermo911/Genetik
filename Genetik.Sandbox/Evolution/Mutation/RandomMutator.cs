using Genetik.Core;
using Genetik.Evolution.Tools;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Mutation;

public class RandomMutator : IMutator<Vec2>
{
    private Random _random = new Random(Guid.NewGuid().GetHashCode());
    private double _probability;
    private double _maxAbsX;
    private double _maxAbsY;

    public RandomMutator(double probability, double maxAbsX, double maxAbsY)
    {
        _probability = probability;
        _maxAbsX = maxAbsX;
        _maxAbsY = maxAbsY;
    }

    public Genome<Vec2> Mutate(Genome<Vec2> genome)
    {
        var genes = new Vec2[genome.Length];
        for (int i = 0; i < genome.Length; i++)
        {
            genes[i] = genome.Genes[i];
            if (_random.NextDouble() < _probability)
                genes[i] = new Vec2(_random.NextDouble(_maxAbsX), _random.NextDouble(_maxAbsY));
        }

        return new Genome<Vec2>(genes);
    }
}