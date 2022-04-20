namespace Genetik.Core;

public class Population<TGene>
{
    private Genome<TGene>[] _genomes;

    public Population(
        IEnumerable<Genome<TGene>> genomes,
        IFitnessEvaluator<TGene> fitnessEvaluator)
    {
        _genomes = genomes
            .OrderByDescending(fitnessEvaluator.GetFitness)
            .ToArray();
    }

    public IReadOnlyList<Genome<TGene>> OrderedGenomes => _genomes;

    public int Size => _genomes.Length;
}