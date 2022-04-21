namespace Genetik.Core;

public class Genome<TGene>
{
    private readonly TGene[] _genes;

    public Genome(IEnumerable<TGene> genes)
    {
        _genes = genes.ToArray();
    }

    public int Length => _genes.Length;

    public IReadOnlyList<TGene> Genes => _genes;
}