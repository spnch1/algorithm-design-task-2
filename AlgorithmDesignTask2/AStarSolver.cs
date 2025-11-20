namespace AlgorithmDesignTask2;

public class AStarSolver : ISolver
{
    public SearchResult Solve(State initialState, Func<State, int> heuristic, bool debug = false)
    {
        var result = new SearchResult();
        var startTime = DateTime.Now;
        
        var openSet = new PriorityQueue<State, int>();
        var gScore = new Dictionary<State, int>();
        var closedSet = new HashSet<State>();

        gScore[initialState] = 0;
        openSet.Enqueue(initialState, heuristic(initialState));
        
        int maxMemory = 0;
        int iterations = 0;

        while (openSet.Count > 0)
        {
            if ((DateTime.Now - startTime).TotalMinutes >= 30)
            {
                break;
            }

            int currentMemory = openSet.Count + closedSet.Count;
            if (currentMemory > maxMemory) maxMemory = currentMemory;
            
            State current = openSet.Dequeue();
            iterations++;
            
            if (debug)
            {
                Console.WriteLine($"--- DEBUG MODE (A*) ---");
                Console.WriteLine($"Iteration: {iterations}");
                Console.WriteLine($"Depth (G): {gScore[current]}");
                Console.WriteLine($"Heuristic (H): {heuristic(current)}");
                Console.WriteLine($"Total Cost (F): {gScore[current] + heuristic(current)}");
                Console.WriteLine($"Open Set: {openSet.Count}, Closed Set: {closedSet.Count}");
                Console.WriteLine();
                
                current.PrintBoard();

                Console.WriteLine("Press any key to continue, 'S' to skip debug...");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.S)
                {
                    debug = false;
                    Console.WriteLine("Debug mode disabled. Finishing run...");
                }
            }

            if (heuristic(current) == 0)
            {
                result.Success = true;
                result.Solution = current;
                result.SolutionDepth = gScore[current];
                result.Iterations = iterations;
                result.MaxMemoryStates = maxMemory;
                result.TimeElapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                return result;
            }

            closedSet.Add(current);

            foreach (var neighbor in current.GetNeighbors())
            {
                result.GeneratedStates++;
                
                if (closedSet.Contains(neighbor))
                    continue;

                int tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeG;
                    int f = tentativeG + heuristic(neighbor);
                    openSet.Enqueue(neighbor, f);
                }
            }
        }

        result.Success = false;
        result.MaxMemoryStates = maxMemory;
        result.TimeElapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
        return result;
    }
}
