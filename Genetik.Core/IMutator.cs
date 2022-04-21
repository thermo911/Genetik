namespace Genetik.Core;

public interface IMutator<TGene>
{
    /// <summary>
    /// Perform mutations in genes of given genome.
    /// </summary>
    /// <returns>
    /// a new genome - result of mutations in <c>genome</c>
    /// </returns>
    Genome<TGene> Mutate(Genome<TGene> genome);
}