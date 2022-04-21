namespace Genetik.Core;

public interface ICrosser<TGene>
{
    /// <summary>
    /// Crosses genes of given genomes.
    /// </summary>
    /// <returns>Pair of new genomes produced while crossing.</returns>
    Tuple<Genome<TGene>, Genome<TGene>> CrossGenomes(Genome<TGene> genome1, Genome<TGene> genome2);
}