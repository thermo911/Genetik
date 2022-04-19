namespace Genetik.Core;

public class Population<TGene>
{
    private Genome<TGene>[] _genomes;

    public Population(IEnumerable<Genome<TGene>> genomes)
    {
        _genomes = genomes.ToArray();
    }

    public IReadOnlyList<Genome<TGene>> Genomes => _genomes;
}