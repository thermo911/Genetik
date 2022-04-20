using Genetik.Core;
using Genetik.Evolution.Tools;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Generation;

public class RandomGenerator : IGenomesGenerator<Vec2>
{
    private Random _random = new Random(Guid.NewGuid().GetHashCode());
    private double _maxAbsX;
    private double _maxAbsY;

    public RandomGenerator(double maxAbsX, double maxAbsY)
    {
        _maxAbsX = maxAbsX;
        _maxAbsY = maxAbsY;
    }

    public IEnumerable<Genome<Vec2>> GenerateGenomes(int genomeLength, int genomesCount)
    {
        var genomes = new List<Genome<Vec2>>(genomesCount);
        for (int i = 0; i < genomesCount; i++)
        {
            var genes = new List<Vec2>(genomeLength);
            for (int j = 0; j < genomeLength; j++)
            {
                genes.Add(new Vec2(
                    _random.NextDouble(_maxAbsX),
                    _random.NextDouble(_maxAbsY)));
            }
            genomes.Add(new Genome<Vec2>(genes));
        }

        return genomes;
    }
}