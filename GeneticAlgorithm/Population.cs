using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        private int _size;
        public List<Genome> _genomes { get; set; }

        public Population(List<Genome> population)
        {
            this._genomes = population ?? throw new NullReferenceException();
            this._size = population.Count;
        }

        public Population(int size, int genomeSize)
        {
            if (!(genomeSize > 0)) throw new ArgumentException();
            this._size = size > 0 ? size : throw new ArgumentException();
            this._genomes = new List<Genome>(size);
            for (int i = 0; i < size; i++) 
                _genomes.Add(new Genome(genomeSize));
        }

        public void NextGeneration()
        {
            select();
            crossover();
            mutate();
        }

        private void select()
        {
            WheelOfFortune wheel = new WheelOfFortune(_genomes);
            List<Genome> tmp = new List<Genome>();
            Genome result;
            for (int i = 0; i < _genomes.Count / 2; i++)
            {
                tmp.Add(result = wheel.Spin());
                tmp.Add(result.Copy());
            }
            _genomes = tmp;
        }

        public void pair()
        {
            Random random = new Random();
            int index;
            for (int i = 0; i < _genomes.Count; i++)
            {
                index = random.Next(0, _size);
                if (_genomes[i].Pair == null && _genomes[index].Pair == null && i != index)
                {
                    _genomes[i].Pair = _genomes[index];
                    _genomes[index].Pair = _genomes[i];
                }
                else if (_genomes[i].Pair != null) continue;
                else i--;
            }
        }

        private void crossover()
        {
            pair();
            Genome tmp1, tmp2;
            Random random = new Random();
            int index;
            for (int i = 0; i < _genomes.Count; i++)
            {
                index = random.Next(0, _genomes[i].Genes.Count);
                tmp1 = _genomes[i].Copy();
                tmp2 = _genomes[i].Pair.Copy();
                for (int j = index; j < _genomes[i].Genes.Count; j++)
                {
                    _genomes[i].Genes[j] = tmp2.Genes[j];
                    _genomes[i].Pair.Genes[j] = tmp1
                        
                        
                        
                        
                        
    .Genes[j];
                }
            }
        }

        private void mutate()
        {

        }

        private void swap()
        {

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
