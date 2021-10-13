using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class Genome
    {
        /// <summary>
        /// Creates random genome with random arrangement of ZEROs and ONEs.
        /// </summary>
        /// <param name="length">Length of genome to be created.</param>
        public Genome(int length)
        {
            genome = new Gene[length];
            Random random = new Random();
            for (int i = 0; i < genome.Length; i++)
                genome[i] = random.Next(0, 1) == 0 ? Gene.ZERO : Gene.ONE;
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
            genome = new Gene[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == 0) genome[i] = Gene.ZERO;
                else if (elements[i] == 1) genome[i] = Gene.ONE;
                else throw new ArgumentException();
            }
        }

        private Gene[] genome;
    }
}
