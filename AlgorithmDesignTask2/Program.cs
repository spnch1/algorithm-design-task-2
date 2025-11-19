using System.Diagnostics;

namespace AlgorithmDesignTask2;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("8 Queens Solver");

        string? algorithm = null;
        string? heuristicName = null;
        double? coolingK = null;
        int experimentCount = 20;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i].ToLower())
            {
                case "-a":
                case "--algorithm":
                    if (i + 1 < args.Length) algorithm = args[++i].ToLower();
                    break;
                case "-h":
                case "--heuristic":
                    if (i + 1 < args.Length) heuristicName = args[++i].ToLower();
                    break;
                case "-k":
                case "--cooling":
                    if (i + 1 < args.Length && double.TryParse(args[++i], out double k)) coolingK = k;
                    break;
                case "-c":
                case "--count":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out int c)) experimentCount = c;
                    break;
                case "--help":
                    PrintUsage();
                    return;
            }
        }

        if (string.IsNullOrEmpty(algorithm) || string.IsNullOrEmpty(heuristicName))
        {
            Console.WriteLine("Please specify both algorithm and heuristic.");
            PrintUsage();
            return;
        }

        if (coolingK.HasValue && algorithm != "anneal")
        {
            Console.WriteLine("Warning: Cooling coefficient (-k) is ignored for algorithms other than 'anneal'.");
        }

        int n = 8;
        var initialStates = new List<State>();
        for (int i = 0; i < experimentCount; i++) initialStates.Add(new State(n));

        ISolver? solver = null;
        switch (algorithm)
        {
            case "astar": solver = new AStarSolver(); break;
            case "anneal": 
                solver = new SimulatedAnnealingSolver(coolingK ?? 0.01); 
                break;
            default:
                Console.WriteLine($"Unknown algorithm: {algorithm}");
                PrintUsage();
                return;
        }

        Func<State, int>? heuristic = null;
        switch (heuristicName)
        {
            case "f2": heuristic = Heuristics.F2; break;
            case "custom": heuristic = Heuristics.Custom; break;
            default:
                Console.WriteLine($"Unknown heuristic: {heuristicName}");
                PrintUsage();
                return;
        }

        string configName = $"{algorithm.ToUpper()} ({heuristicName})";
        if (algorithm == "anneal") configName += $" [K={coolingK ?? 0.01}]";
        
        RunSeries(configName, solver, heuristic, initialStates);
        
        Console.WriteLine("\nDone.");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("\nUsage:");
        Console.WriteLine("  dotnet run -- -a <algorithm> -h <heuristic> [-k <cooling_coeff>] [-c <count>]");
        Console.WriteLine("\nOptions:");
        Console.WriteLine("  -a, --algorithm   astar | anneal");
        Console.WriteLine("  -h, --heuristic   f2 | custom");
        Console.WriteLine("  -k, --cooling     Cooling coefficient (default 0.01). Only for anneal.");
        Console.WriteLine("  -c, --count       Number of experiments (default 20).");
        Console.WriteLine("\nExample:");
        Console.WriteLine("  dotnet run -- -a anneal -h custom -k 0.001 -c 50");
    }

    private static void RunSeries(string name, ISolver solver, Func<State, int> heuristic, List<State> initialStates)
    {
        Console.WriteLine($"\n--- Running Series: {name} ---");
        
        double totalSteps = 0;
        double totalDeadEnds = 0;
        double totalGenerated = 0;
        double totalMemory = 0;
        double totalTime = 0;
        int successCount = 0;

        Console.WriteLine($"{"Exp",-5} | {"Status",-10} | {"Steps",-10} | {"Gen",-10} | {"Mem",-10} | {"Time(s)",-10}");
        Console.WriteLine(new string('-', 70));

        for (int i = 0; i < initialStates.Count; i++)
        {
            var startState = initialStates[i];
            var result = solver.Solve(startState, heuristic);

            totalSteps += result.Steps;
            totalDeadEnds += result.DeadEnds;
            totalGenerated += result.GeneratedStates;
            totalMemory += result.MaxMemoryStates;
            totalTime += result.TimeElapsedSeconds;
            if (result.Success) successCount++;

            Console.WriteLine($"{i+1,-5} | {(result.Success ? "Solved" : "Fail"),-10} | {result.Steps,-10} | {result.GeneratedStates,-10} | {result.MaxMemoryStates,-10} | {result.TimeElapsedSeconds,-10:F4}");
        }

        int n = initialStates.Count;
        Console.WriteLine(new string('-', 70));
        Console.WriteLine($"Average Stats for {name}:");
        Console.WriteLine($"  Success Rate: {successCount}/{n}");
        Console.WriteLine($"  Avg Steps:    {totalSteps / n:F2}");
        Console.WriteLine($"  Avg DeadEnds: {totalDeadEnds / n:F2}");
        Console.WriteLine($"  Avg Gen:      {totalGenerated / n:F2}");
        Console.WriteLine($"  Avg Memory:   {totalMemory / n:F2}");
        Console.WriteLine($"  Avg Time:     {totalTime / n:F4} s");
    }
}