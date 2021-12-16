using System;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using ScottPlot;

namespace GeneticAlgorithm
{
    public class Population
    {
        private readonly int _size;
        private readonly int _chromosomeSize;

        private int _generationCount = 0;

        public List<Chromosome> Chromosomes;
        private List<Chromosome> _newPopulation;

        private readonly double _probabilityOfCrossover;
        private readonly double _probabilityOfMutation;

        private Reproduction _wheelOfFortune;

        private readonly Function _fitness;
        private readonly int _start;
        private readonly int _end;

        private readonly Random _random;

        private double _cmin = 0;

        // DEBUG
        private int _mutations;
        private int _crossovers;

        private List<double> _maxes, _avges, _mins;
        private List<double> _generations;


        public Population(int popSize, double pMutation, double pCrossover, Function fitness, int start, int end)
        {
            _random = new();

            this._probabilityOfMutation = (pMutation >= 0 && 1 > pMutation) ? pMutation : throw new ArgumentException("Probability of mutation has to be in range <0, 1>.");
            this._probabilityOfCrossover = (pCrossover >= 0 && 1 > pCrossover) ? pCrossover : throw new ArgumentException("Probability of crossover has to be in range <0, 1>.");

            if (popSize <= 0) throw new ArgumentException("Population size cannot be less than or equal 0.");
            else if (popSize % 2 != 0) throw new ArgumentException("Population size should be even.");
            else this._size = popSize;

            this.Chromosomes = new List<Chromosome>(popSize);
            this._fitness = fitness;

            if (start < end)
            {
                _start = start;
                _end = end;
            }
            else throw new ArgumentException(String.Format("Begining of domain (%d) should be less number than the end of it (%d).", start, end));

            _chromosomeSize = (int)Math.Ceiling(Math.Log2(end - start));

            for (int i = 0; i < popSize; i++)
                 Chromosomes.Add(new Chromosome(_chromosomeSize));

            _maxes = new List<double>();
            _avges = new List<double>();
            _mins = new List<double>();
            _generations = new List<double>();

            Console.WriteLine(String.Format("mut: {0} crs: {1}", pMutation, pCrossover));
        }

        public void NextGeneration()
        {
            _cmin = Goal(WorstGoal());

            _wheelOfFortune = new Reproduction(this);

            _mutations = 0;
            _crossovers = 0;

            _newPopulation = new List<Chromosome>();
            Crossover();
            Mutate();
            Chromosomes = _newPopulation;

            _maxes.Add(Fitness(BestGoal()));
            _avges.Add(AverageFitness());
            _mins.Add(Fitness(WorstGoal()));

            PrintGenerationStats();

            _generations.Add(++_generationCount);
        }

        private Chromosome Select()
        {
            return _wheelOfFortune.Spin();
        }

        private void Crossover()
        {
            for (int i = 0; i < this._size / 2; i++)
            {
                Crossover(Select(), Select());
            }
        }

        private void Crossover(Chromosome parent1, Chromosome parent2)
        {
            if (_probabilityOfCrossover > _random.NextDouble())
            {
                _crossovers++;
                List<Gene> genes1 = new(parent1.Length), genes2 = new(parent2.Length);
                int crossingIndex = _random.Next(1, parent1.Length);

                for (int i = 0; i < parent1.Length; i++)
                {
                    genes1.Insert(i, i < crossingIndex ? parent1.GetGene(i) : parent2.GetGene(i));
                    genes2.Insert(i, i < crossingIndex ? parent2.GetGene(i) : parent1.GetGene(i));
                }

                _newPopulation.Add(new Chromosome(genes1));
                _newPopulation.Add(new Chromosome(genes2));
            } 
            else
            {
                _newPopulation.Add(new Chromosome(parent1.Genes));
                _newPopulation.Add(new Chromosome(parent2.Genes));
            }
        }

        private void Mutate()
        {
            for (int i = 0; i < _newPopulation.Count; i++)
                for (int j = 0; j < _newPopulation[i].Genes.Count; j++)
                    if (_random.NextDouble() < _probabilityOfMutation)
                    {
                        _mutations++;
                        _newPopulation[i].Genes[j] = _newPopulation[i].Genes[j].Equals(Gene.ONE) ? Gene.ZERO : Gene.ONE;
                    }
        }

        private double Goal(Chromosome chromosome)
        {
            double result = _fitness.calculate(chromosome.Decode(_start, _end));
            if (double.IsNaN(result))
            {
                throw new ArgumentException(String.Format(
                    "Function {0} is invalid: for argument {1} returns NaN.",
                    _fitness.getFunctionName() + "(" + _fitness.getParameterName(0) + ")=" + _fitness.getFunctionExpressionString(),
                    chromosome.Decode(_start, _end)
                ));
            }
            return result;
        }

        public double Fitness(Chromosome chromosome)
        {
            double result = Math.Abs(_cmin) + Goal(chromosome);
            return result > 0 ? result : 0;
        }

        public Chromosome BestGoal()
        {
            Chromosome best = Chromosomes[0];
            for (int i = 1; i < _size; i++)
                best = Goal(Chromosomes[i]) > Goal(best) ? Chromosomes[i] : best;
            return best;
        }

        public Chromosome WorstGoal()
        {
            Chromosome worst = Chromosomes[0];
            for (int i = 1; i < _size; i++)
                worst = Goal(Chromosomes[i]) < Goal(worst) ? Chromosomes[i] : worst;
            return worst;
        }

        public double AverageFitness()
        {
            return _wheelOfFortune.Sum / _size;
        }

        private void PrintGenerationStats()
        {
            Console.Write(string.Format("" +
                "nr: {0:d4}   " +
                "best: {1}   " +
                "fmax: {2:e6}   " +
                "favg: {3:e6}   " +
                "fmin: {4:e6}   " +
                "mut: {5}   " +
                "crs: {6}\n",
                _generationCount,
                BestGoal(),
                _maxes[_generationCount],
                _avges[_generationCount],
                _mins[_generationCount],
                _mutations,
                _crossovers
            ));
        }

        public void Plot()
        {
            Plot plt = new Plot(1000, 1000);
            var values = new List<double>();
            var args = new List<double>();
            for (int i = _start; i <= _end; i++)
            {
                args.Add(i);
                values.Add(_fitness.calculate(i));
            }

            plt.AddScatter(args.ToArray(), values.ToArray());
            plt.SaveFig("./../../../../../results/function.png");

            plt = new Plot(1000, 1000);
            plt.AddScatter(_generations.ToArray(), _maxes.ToArray());
            plt.AddScatter(_generations.ToArray(), _avges.ToArray());
            plt.AddScatter(_generations.ToArray(), _mins.ToArray());
            plt.SaveFig("./../../../../../results/report.png");
        }

        class Reproduction
        {
            private List<Field> _fields;
            private Population _population;
            private Random _random;
            public double Sum { get; }

            public Reproduction(Population population)
            {
                _random = new();
                _population = population;
                _fields = new List<Field>();
                Sum = 0.0;

                foreach (Chromosome genome in _population.Chromosomes)
                {
                    Sum += _population.Fitness(genome);
                    _fields.Add(new Field(Sum - _population.Fitness(genome), Sum, genome));
                }
            }

            public Chromosome Spin()
            {
                double result = _random.NextDouble() * Sum;
                foreach (Field field in _fields)
                    if (field.Start <= result && result < field.End) return field.Gen;
                return null;
            }

            class Field
            {
                public double Start { get; }
                public double End { get; }
                public Chromosome Gen { get; }

                public Field(double start, double end, Chromosome gen)
                {
                    this.Start = start;
                    this.End = end;
                    this.Gen = gen;
                }
            }
        }
    }
}