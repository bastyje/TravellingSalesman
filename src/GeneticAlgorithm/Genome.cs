using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class Genome
    {
        public List<Gene> Genes { get; }
        public int Adaptation { get; }
        public Genome Pair { get; set; }
        public int Length { get; }

        public Genome(List<Gene> genes)
        {
            Genes = genes ?? genes;
        }

        /// <summary>
        /// Creates random genome with random arrangement of ZEROs and ONEs.
        /// </summary>
        /// <param name="length">Length of genome to be created.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when passed "length" argument is equal or less than 0.
        /// </exception>
        public Genome(int length)
        {
            if (!(length > 0)) throw new ArgumentException();
            Genes = new List<Gene>(length);
            Random random = new Random();
            this.Length = length;
            int i;
            for (i = 0; i < length; i++)
                Genes.Add(random.Next(0, 2) == 0 ? Gene.ZERO : Gene.ONE);
        }

        /// <summary>
        /// Creates genome with arrangement of ZEROs and ONEs the same way as in the "elements" array.
        /// </summary>
        /// <param name="elements">
        /// Creation of Genome class will be based on the order of elements in the array.
        /// Array can consist of 0 and 1 values only. Otherwise throws ArgumentException.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when there is an element other than "0" or "1" in "elements" array.
        /// </exception>
        public Genome(int[] elements)
        {
            Genes = new List<Gene>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == 0) Genes.Add(Gene.ZERO);
                else if (elements[i] == 1) Genes.Add(Gene.ONE);
                else throw new ArgumentException();
            }
        }

        public Genome Copy()
        {
            return (Genome) this.MemberwiseClone();
        }
    }
}

