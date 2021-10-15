using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        private int _size;
        public List<Genome> Genomes { get; set; }
        private double _probabilityOfMutation;

        public Population(List<Genome> population, double probability)
        {
            this._probabilityOfMutation = (probability >= 0 || 1 > probability) 
                ? probability : throw new ArgumentException();
            this.Genomes = population ?? throw new NullReferenceException();
            this._size = population.Count;
        }

        public Population(int size, int genomeSize, double probability)
        {
            if (!(genomeSize > 0)) throw new ArgumentException();
            this._probabilityOfMutation = (probability >= 0 || 1 > probability)
                ? probability : throw new ArgumentException();
            this._size = size > 0 ? size : throw new ArgumentException();
            this.Genomes = new List<Genome>(size);
            for (int i = 0; i < size; i++) 
                Genomes.Add(new Genome(genomeSize));
        }

        public void NextGeneration()
        {
            select();
            crossover();
            mutate();
        }

        private void select()
        {
            WheelOfFortune wheel = new WheelOfFortune(Genomes);
            List<Genome> tmp = new List<Genome>();
            Genome result;
            for (int i = 0; i < Genomes.Count / 2; i++)
            {
                tmp.Add(result = wheel.Spin());
                tmp.Add(result.Copy());
            }
            Genomes = tmp;
        }

        private void pair()
        {
            Random random = new Random();
            int index;
            for (int i = 0; i < Genomes.Count; i++)
            {
                index = random.Next(0, _size);
                if (Genomes[i].Pair == null && Genomes[index].Pair == null && i != index)
                {
                    Genomes[i].Pair = Genomes[index];
                    Genomes[index].Pair = Genomes[i];
                }
                else if (Genomes[i].Pair != null) continue;
                else i--;
            }
        }

        private void crossover()
        {
            pair();
            Genome tmp1, tmp2;
            Random random = new Random();
            int index;
            for (int i = 0; i < Genomes.Count; i++)
            {
                index = random.Next(0, Genomes[i].Genes.Count);
                tmp1 = Genomes[i].Copy();
                tmp2 = Genomes[i].Pair.Copy();
                for (int j = index; j < Genomes[i].Genes.Count; j++)
                {
                    Genomes[i].Genes[j] = tmp2.Genes[j];
                    Genomes[i].Pair.Genes[j] = tmp1.Genes[j];
                }
            }
        }

        private void mutate()
        {
            Random random = new Random();
            double randVal;
            for (int i = 0; i < Genomes.Count; i++)
            {
                for (int j = 0; j < Genomes[i].Genes.Count; j++)
                {
                    randVal = random.NextDouble();
                    if (0 <= randVal && randVal < _probabilityOfMutation)
                        Genomes[i].Genes[j] = Genomes[i].Genes[j].Equals(Gene.ONE) 
                            ? Gene.ZERO : Gene.ONE;
                }
            }
        }
    }

    class WheelOfFortune
    {
        private List<Field> _fields;
        private int _sum;

        public WheelOfFortune(List<Genome> genomes)
        {
            _fields = new List<Field>();
            int sum = 0;
            foreach (Genome genome in genomes)
            {
                sum += genome.Adaptation;
                _fields.Add(new Field(sum - genome.Adaptation, sum, genome));
                _sum = sum;
            }
        }

        public Genome Spin()
        {
            Random random = new Random();
            int result = random.Next(0, _sum);
            foreach (Field field in _fields)
                if (field.Start <= result && result < field.End) return field.Gen;
            return null;
        }

        class Field
        {
            public int Start { get; }
            public int End { get; }
            public Genome Gen { get; }

            public Field(int start, int end, Genome gen)
            {
                this.Start = start;
                this.End = end;
                this.Gen = gen;
            }
        }
    }
}
