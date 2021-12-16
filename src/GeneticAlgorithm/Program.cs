using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using org.mariuszgromada.math.mxparser;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand("App uses genetic algorithm to find maximum of function of one variable defined for integers.")
            {
                new Argument<int>(
                    "pop-size",
                    description: "Size of population. Only integer values allowed."
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
                    "end-cond",
                    description: "Number of generations of population without progress (the best adaptation of genome has " +
                    "not risen since end-cond number of generations."
                    ),
                new Argument<string>(
                    "fit-func",
                    description: "Function of fitness of chromosome. Search process is based on this function values." +
                    "This program supports one variable functions. Variable should be named 'x'. Example of input: " +
                    "f(x) = -0.5*x^2+10x+13"
                    ),
                new Argument<int>(
                    "dom-start",
                    description: "Begenning of domain (inclusive) of fitness function. Only integer values."
                    ),
                new Argument<int>(
                    "dom-end",
                    description: "End of domain (inclusive) of fitness function. Only integer values."
                    )
            };

            rootCommand.Handler = CommandHandler.Create<int, double, double, int, string, int, int>(Search);

            return rootCommand.InvokeAsync(args).Result;
        }
        public static void Search(int popSize, double mutProb, double crsProb, int endCond, string fitFunc, int domStart, int domEnd)
        {
            try
            {
                long start = domStart, end = domEnd;
                if (Math.Abs(end - start) > int.MaxValue)
                {
                    throw new ArgumentException(String.Format("Maximum range of domain is equal {0}, not {1}.", int.MaxValue, (end - start).ToString()));
                }

                Console.WriteLine(fitFunc);

                Function fitness = new(fitFunc);


                Population population = new(popSize, mutProb, crsProb, fitness, domStart, domEnd);

                for (int i = 0; i < endCond; i++)
                    population.NextGeneration();

                Chromosome best = population.BestGoal();
                Console.WriteLine(string.Format("\nBest chromosome: {0}   value of function: {1}   for x = {2}",
                    population.BestGoal().ToString(),
                    fitness.calculate(population.BestGoal().Decode(domStart, domEnd)),
                    best.Decode(domStart, domEnd)
                ));
                population.Plot();
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
