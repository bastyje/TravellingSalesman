using System;
using System.Collections.Generic;
using ScottPlot;
using System.Drawing;

namespace GeneticAlgorithm
{
    public class Population
    {
        //
        // Genetic algorithm parameters
        //
        private readonly int _size;
        private readonly int _chromosomeSize;
        public List<Chromosome> Chromosomes;
        private List<Chromosome> _newPopulation;
        private readonly double _crsProb;
        private readonly double _mutProb;
        private double _cmax;
        //
        // Technical objects
        //
        private Reproduction _wheelOfFortune;
        private readonly Random _random;
        private readonly List<City> _cities;
        //
        // Statistics
        //
        private List<double> _maxes, _avges, _mins;
        private List<double> _generations;
        private int _generationCount = 0;

        public Population(int popSize, double pMutation, double pCrossover, List<City> cities)
        {
            _random = new Random();

            _mutProb = (pMutation >= 0 && 1 > pMutation) ? pMutation : throw new ArgumentException("Probability of mutation has to be in range <0, 1>.");
            _crsProb = (pCrossover >= 0 && 1 > pCrossover) ? pCrossover : throw new ArgumentException("Probability of crossover has to be in range <0, 1>.");

            if (popSize <= 0) throw new ArgumentException("Population size cannot be less than or equal 0.");
            else if (popSize % 2 != 0) throw new ArgumentException("Population size should be even.");
            else _size = popSize;

            Chromosomes = new List<Chromosome>(popSize);

            _cities = cities;

            if (cities.Count > 1)
                _chromosomeSize = cities.Count;
            else
                throw new ArgumentException(string.Format("It is impossible to count distance between {0} cities.", cities.Count));

            for (int i = 0; i < popSize; i++)
                Chromosomes.Add(new Chromosome(_chromosomeSize));

            _maxes = new List<double>();
            _avges = new List<double>();
            _mins = new List<double>();
            _generations = new List<double>();
        }

        public double[] NextGeneration()
        {
            _cmax = Goal(WorstGoal());

            _wheelOfFortune = new Reproduction(this);

            _newPopulation = new List<Chromosome>();
            Crossover();
            Mutate();
            Chromosomes = _newPopulation;

            _maxes.Add(Goal(BestGoal()));
            _avges.Add(AverageGoal());
            _mins.Add(Goal(WorstGoal()));
            _generations.Add(++_generationCount);

            return new double[] { _maxes[_generationCount - 1], _avges[_generationCount - 1], _mins[_generationCount - 1] };
        }

        public void Plot(string path)
        {
            Plot plot = new Plot(1000, 1000);

            plot.AddScatter(_generations.ToArray(), _maxes.ToArray(), label: "Best");
            plot.AddScatter(_generations.ToArray(), _avges.ToArray(), label: "Average");
            plot.AddScatter(_generations.ToArray(), _mins.ToArray(), label: "Worst");
            plot.Legend(location: Alignment.UpperRight);
            plot.SaveFig(path + "/report.png");

            plot = new Plot(1000, 1000);

            Font font = new Font("Serif", 20);

            var bubblePlt = plot.AddBubblePlot();

            foreach (City city in _cities)
            {
                bubblePlt.Add(city.X, city.Y, 10, System.Drawing.Color.FromArgb(_random.Next(256), _random.Next(256), _random.Next(256)), 1, System.Drawing.Color.Transparent);
                plot.AddText(string.Format("{0} ({1}, {2})", city.Name, city.X, city.Y), city.X, city.Y, 18, Color.Black);
            }

            plot.SaveFig(path + "/map.png");
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
            if (_random.NextDouble() < _crsProb)
            {
                int startingGene = parent1.Genes[0];
                int gene = parent2.Genes[0];

                Chromosome child1 = new Chromosome(_chromosomeSize), child2 = new Chromosome(_chromosomeSize);
                bool[] visited = new bool[_chromosomeSize];
                visited[0] = true;

                // DEBUG
                bool[] used1 = new bool[_chromosomeSize];
                bool[] used2 = new bool[_chromosomeSize];


                do
                {
                    for (int i = 1; i < _chromosomeSize; i++)
                    {

                        if (gene == parent1.Genes[i])
                        {
                            gene = parent2.Genes[i];
                            visited[i] = true;
                        }

                        if (gene == startingGene)
                            break;
                    }
                } while (startingGene != gene);

                for (int i = 0; i < _chromosomeSize; i++)
                {
                    if (!visited[i])
                    {
                        child1.Genes[i] = parent2.Genes[i];
                        child2.Genes[i] = parent1.Genes[i];
                    }
                    else
                    {
                        child1.Genes[i] = parent1.Genes[i];
                        child2.Genes[i] = parent2.Genes[i];
                    }
                    used1[parent1.Genes[i]] = true;
                    used2[parent2.Genes[i]] = true;

                }
                parent1 = child1;
                parent2 = child2;
            }
            _newPopulation.Add(parent1);
            _newPopulation.Add(parent2);
        }

        private void Mutate()
        {
            foreach (Chromosome chromosome in _newPopulation)
            {
                for (int i = 0; i < _chromosomeSize; i++)
                {
                    if (_random.NextDouble() < _mutProb)
                    {
                        chromosome.SwapGenes(i, _random.Next(0, _chromosomeSize));
                    }
                }
            }
        }

        public double Goal(Chromosome chromosome)
        {
            double distance = 0;
            for (int i = 0; i < _chromosomeSize - 1; i++)
            {
                distance += Math.Sqrt(
                    Math.Pow(_cities[chromosome.Genes[i]].X - _cities[chromosome.Genes[i + 1]].X, 2) +
                    Math.Pow(_cities[chromosome.Genes[i]].Y - _cities[chromosome.Genes[i + 1]].Y, 2));
            }

            return distance;
        }

        private double Fitness(Chromosome chromosome)
        {
            double goal = Goal(chromosome);
            return _cmax > goal ? _cmax - goal : 0;
        }

        public Chromosome BestGoal()
        {
            Chromosome best = Chromosomes[0];
            for (int i = 1; i < _size; i++)
                best = Goal(Chromosomes[i]) < Goal(best) ? Chromosomes[i] : best;
            return best;
        }

        public Chromosome WorstGoal()
        {
            Chromosome worst = Chromosomes[0];
            for (int i = 1; i < _size; i++)
                worst = Goal(Chromosomes[i]) > Goal(worst) ? Chromosomes[i] : worst;
            return worst;
        }

        public double AverageGoal()
        {
            double sum = 0.0;
            foreach (Chromosome chromosome in Chromosomes)
                sum += Goal(chromosome);

            return sum / _size;
        }
        class Reproduction
        {
            private List<Field> _fields;
            private Population _population;
            private Random _random;
            public double Sum { get; }

            public Reproduction(Population population)
            {
                _random = new Random();
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