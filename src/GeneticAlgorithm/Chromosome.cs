using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class Chromosome
    {
        public List<Gene> Genes { get; }
        public int Length { get; }

        public Chromosome(List<Gene> genes)
        {
            Genes = genes ?? genes;
            Length = genes.Count;
        }

        /// <summary>
        ///     Creates random genome with random arrangement of ZEROs and ONEs.
        /// </summary>
        /// <param name="length">
        ///     Length of genome to be created.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown when passed "length" argument is equal or less than 0.
        /// </exception>
        public Chromosome(int length)
        {
            if (!(length > 0)) throw new ArgumentException("Chromosome length has to be greater than 0.");
            Genes = new List<Gene>(length);
            Random random = new();
            this.Length = length;
            int i;
            for (i = 0; i < length; i++)
                Genes.Insert(i, random.Next(0, 2) == 0 ? Gene.ZERO : Gene.ONE);
        }

        /// <summary>
        ///     Creates genome with arrangement of ZEROs and ONEs the same way as in the "elements" array.
        /// </summary>
        /// <param name="elements">
        ///     Creation of Genome class will be based on the order of elements in the array.
        ///     Array can consist of 0 and 1 values only. Otherwise throws ArgumentException.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown when there is an element other than "0" or "1" in "elements" array.
        /// </exception>
        public Chromosome(int[] elements)
        {
            Genes = new List<Gene>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == 0) Genes.Insert(i, Gene.ZERO);
                else if (elements[i] == 1) Genes.Insert(i, Gene.ONE);
                else throw new ArgumentException("Genes cannot be value different than 0 or 1.");
                this.Length = elements.Length;
            }
        }

        public Chromosome Copy()
        {
            return (Chromosome) this.MemberwiseClone();
        }

        public Gene GetGene(int index)
        {
            return Genes[index];
        }

        public int Decode(int start, int end)
        {
            int decodedValue = 0;
            for (int i = 0, bit = Length - 1; i < Length; i++, bit--)
                decodedValue += Genes[i] == Gene.ONE ? (int) Math.Pow(2, bit) : 0;

            return ((int) (start + (end - start) / (Math.Pow(2, Length) - 1) * decodedValue));
        }

        // DEBUG
        public override string ToString()
        {
            String chain = "";
            for (int i = 0; i < Length; i++)
                chain += Genes[i] == Gene.ONE ? "1" : "0";

            return chain;
        }
    }
}