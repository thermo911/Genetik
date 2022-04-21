// See https://aka.ms/new-console-template for more information

using Genetik.Evolution.Blueprints.Fitness;
using Genetik.Evolution.Blueprints.Processes;
using Genetik.Sandbox.Evolution.Crossing;
using Genetik.Sandbox.Evolution.Fitness;
using Genetik.Sandbox.Evolution.Generation;
using Genetik.Sandbox.Evolution.Mutation;
using Genetik.Sandbox.Logic;
using Genetik.Sandbox.SFML;

int generationSize = 10000;
int genomeLength = 10;

double elitismRate = 0.15;

double w = 1.0;
double maxAbsX = w / 5;



var field = new Field(w, w, new Circle[] { });

var generator = new RandomGenerator(maxAbsX, maxAbsX);

var evaluator = new ComplexFitnessEvaluator<Vec2>
    .Builder(new PathLengthEvaluator(field), 1.0)
    .AddWeightedEvaluator(new DistanceToFinishEvaluator(field), 3.0)
    .AddCriteria(new WholePathInsideCriteria(field))
    .AddCriteria(new NoCollisionCriteria(field))
    .Build();

var crosser = new RandomSinglePointCrosser();

var mutator = new RandomMutator(0.01, maxAbsX, maxAbsX);

var evolution = new ElitismEvolution<Vec2>(
    elitismRate,
    genomeLength,
    generationSize,
    generator,
    evaluator,
    crosser,
    mutator);

EvolutionDrawer drawer = new EvolutionDrawer(evolution, field, 600);
drawer.Start();