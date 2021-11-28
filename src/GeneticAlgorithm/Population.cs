using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        private readonly int _size;
        public List<Chromosome> Chromosomes { get; }
        private List<Chromosome> _newPopulation;
        private readonly double _probabilityOfCrossover;
        private readonly double _probabilityOfMutation;
        private Reproduction _wheelOfFortune;

        public Population(List<Chromosome> population, double pMutation, double pCrossover)
        {
            this._probabilityOfMutation = (pMutation >= 0 || 1 > pMutation) ? pMutation : throw new ArgumentException("Probability of mutation has to be in range <0, 1>.");
            this._probabilityOfCrossover = (pCrossover >= 0 || 1 > pCrossover) ? pCrossover : throw new ArgumentException("Probability of crossover has to be in range <0, 1>."); 
            this.Chromosomes = population ?? throw new NullReferenceException();
            this._size = population.Count % 2 == 0 ? population.Count : throw new ArgumentException("Population size should be even.");
        }

        public Population(int popSize, int genomeSize, double pMutation, double pCrossover)
        {
            this._probabilityOfMutation = (pMutation >= 0 || 1 > pMutation) ? pMutation : throw new ArgumentException("Probability of mutation has to be in range <0, 1>.");
            this._probabilityOfCrossover = (pCrossover >= 0 || 1 > pCrossover) ? pCrossover : throw new ArgumentException("Probability of crossover has to be in range <0, 1>.");

            if (popSize <= 0) throw new ArgumentException("Population size cannot be less than or equal 0.");
            else if (popSize % 2 != 0) throw new ArgumentException("Population size should be even.");
            else this._size = popSize;

            this.Chromosomes = new List<Chromosome>(popSize);

            if ((genomeSize > 0)) throw new ArgumentException("Genome size cannot be less than or eqal 0.");
            for (int i = 0; i < popSize; i++)
                Chromosomes.Add(new Chromosome(genomeSize));
        }

        public void NextGeneration()
        {
            _wheelOfFortune = new Reproduction(Chromosomes);
            Crossover();
            Mutate();
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
            Random random = new();
            if (_probabilityOfCrossover > random.NextDouble())
            {
                List<Gene> genes1 = new(parent1.Length), genes2 = new(parent2.Length);
                int crossingIndex = random.Next(1, genes1.Count);

                for (int i = 0; i < genes1.Count; i++)
                {
                    genes1.Insert(i, i < crossingIndex ? parent1.GetGene(i) : parent2.GetGene(i));
                    genes2.Insert(i, i < crossingIndex ? parent2.GetGene(i) : parent1.GetGene(i));
                }

                _newPopulation.Add(new Chromosome(genes1));
                _newPopulation.Add(new Chromosome(genes2));
            }
        }

        private void Mutate()
        {
            Random random = new();
            for (int i = 0; i < Chromosomes.Count; i++)
                for (int j = 0; j < Chromosomes[i].Genes.Count; j++)
                    if (random.NextDouble() < _probabilityOfMutation)
                        Chromosomes[i].Genes[j] = Chromosomes[i].Genes[j].Equals(Gene.ONE) ? Gene.ZERO : Gene.ONE;
        }
        class Reproduction
        {
            private readonly List<Field> _fields;
            private readonly int _sum;

            public Reproduction(List<Chromosome> genomes)
            {
                _fields = new List<Field>();
                int sum = 0;
                foreach (Chromosome genome in genomes)
                {
                    sum += genome.Adaptation;
                    _fields.Add(new Field(sum - genome.Adaptation, sum, genome));
                    _sum = sum;
                }
            }

            public Chromosome Spin()
            {
                Random random = new();
                int result = random.Next(0, _sum);
                foreach (Field field in _fields)
                    if (field.Start <= result && result < field.End) return field.Gen;
                return null;
            }

            class Field
            {
                public int Start { get; }
                public int End { get; }
                public Chromosome Gen { get; }

                public Field(int start, int end, Chromosome gen)
                {
                    this.Start = start;
                    this.End = end;
                    this.Gen = gen;
                }
            }
        }
    }
}