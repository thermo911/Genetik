namespace Genetik.Core;

public interface IGenomesGenerator<TGene>
{
    IEnumerable<Genome<TGene>> GenerateGenomes(int genomeLength, int genomesCount);
}