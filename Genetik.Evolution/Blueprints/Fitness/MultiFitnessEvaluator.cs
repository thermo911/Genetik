using Genetik.Core;

namespace Genetik.Evolution.Blueprints.Fitness;

public class MultiFitnessEvaluator<TGene> : IFitnessEvaluator<TGene>
{
    private List<WeightedCriteria> _weightedCriteriaList;
    
    private MultiFitnessEvaluator(List<WeightedCriteria> weightedCriteriaList)
    {
        _weightedCriteriaList = weightedCriteriaList;
    }
    public double GetFitness(Genome<TGene> genome)
    {
        return _weightedCriteriaList.Sum(t => t.GetWeightedFitness(genome));
    }

    public class Builder
    {
        private List<WeightedCriteria> _weightedCriteriaList = new();

        public Builder(IFitnessEvaluator<TGene> evaluator, double weight)
        {
            _weightedCriteriaList.Add(new WeightedCriteria(evaluator, weight));
        }

        public Builder AddCriteria(IFitnessEvaluator<TGene> evaluator, double weight)
        {
            _weightedCriteriaList.Add(new WeightedCriteria(evaluator, weight));
            return this;
        }

        public MultiFitnessEvaluator<TGene> Build()
        {
            NormalizeWeights();
            return new MultiFitnessEvaluator<TGene>(_weightedCriteriaList);
        }

        private void NormalizeWeights()
        {
            double sum = _weightedCriteriaList.Sum(c => Math.Abs(c.Weight));
            for (int i = 0; i < _weightedCriteriaList.Count; i++)
            {
                double normalizedWeight = _weightedCriteriaList[i].Weight / sum;
                _weightedCriteriaList[i].Weight = normalizedWeight;
            }
        }
    }

    private class WeightedCriteria
    {
        public WeightedCriteria(IFitnessEvaluator<TGene> fitnessEvaluator, double weight)
        {
            FitnessEvaluator = fitnessEvaluator;
            Weight = weight;
        }

        public IFitnessEvaluator<TGene> FitnessEvaluator { get; }

        public double Weight { get; set; }

        public double GetWeightedFitness(Genome<TGene> genome)
            => FitnessEvaluator.GetFitness(genome) * Weight;
    }
}