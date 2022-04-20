using Genetik.Core;

namespace Genetik.Evolution.Blueprints.Fitness;

public interface ICriteria<TGene>
{
    bool MeetCriteria(Genome<TGene> genome);
}