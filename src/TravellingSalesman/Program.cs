using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand("App uses genetic algorithm to solve Blind Travelling Salesman problem.")
            {
                new Argument<int>(
                    "pop-size",
                    description: "Size of population. Only positive, even integer values allowed."
                ),
                new Argument<double>(
                    "mut-prob",
                    description: "Probability of mutation of single gene. Only values" +
                    " between 0 and 1 allowed."
                ),
                new Argument<double>(
                    "crs-prob",
                    description: "Probability of crossover of two chromosomes. Only values" +
                    " between 0 and 1 allowed."
                ),
                new Argument<int>(
                    "iter",
                    description: "Number of algorithm iterations. Only values higher or equal 0."
                    ),
                new Argument<string>(
                    "path",
                    description: "Path to file with cities coordinates."
                    )

            };

            rootCommand.Handler = CommandHandler.Create<int, double, double, int, string>(Search);

            return rootCommand.InvokeAsync(args).Result;
        }
        public static void Search(int popSize, double mutProb, double crsProb, int iter, string path)
        {
            try
            {
                StreamReader reader = new(path);

                Regex regex = new(@"^\w+ \d+ \d+$");

                List<City> cities = new();

                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    if (regex.IsMatch(line))
                        cities.Add(new City(line.Split(" ")[0], int.Parse(line.Split(" ")[1]), int.Parse(line.Split(" ")[2])));
                    else
                        throw new ArgumentException(string.Format("Wrong file format in line {0}: {1}.", cities.Count + 1, line));
                }
                Population population = new(popSize, mutProb, crsProb, cities);

                for (int i = 0; i < iter; i++)
                    population.NextGeneration();

                Chromosome chromosome = population.BestGoal();
                for (int i = 0; i < chromosome.Length; i++)
                    Console.Write(cities[chromosome.Genes[i]].Name);
                Console.Write("\n");

                population.Plot("./../../../../../results");
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
    static class CommandExtensions
    {
        public static Command WithHandler(this Command command, string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var method = typeof(Program).GetMethod(name, flags);

            var handler = CommandHandler.Create(method!);
            command.Handler = handler;
            return command;
        }
    }
}
