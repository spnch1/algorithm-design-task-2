namespace AlgorithmDesignTask2;

public class SimulatedAnnealingSolver : ISolver
{
    private const double K = 0.01;

    public SearchResult Solve(State initialState, Func<State, int> heuristic)
    {
        var result = new SearchResult();
        var startTime = DateTime.Now;
        
        State current = new State(initialState.Queens);
        int currentH = heuristic(current);
        
        State best = current;
        int bestH = currentH;

        int t = 0;
        
        result.MaxMemoryStates = 3; 

        while (true)
        {
            if ((DateTime.Now - startTime).TotalMinutes >= 30)
            {
                result.Steps = t;
                break;
            }

            if (currentH == 0)
            {
                result.Success = true;
                result.Solution = current;
                result.Steps = t;
                result.TimeElapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                return result;
            }

            double T = 1000.0 - K * t;
            
            if (T <= 0)
            {
                result.DeadEnds++;
                result.Steps = t;
                break; 
            }

            State next = current.GetRandomNeighbor();
            result.GeneratedStates++;
            
            int nextH = heuristic(next);
            int deltaE = nextH - currentH;

            if (deltaE < 0)
            {
                current = next;
                currentH = nextH;
                if (currentH < bestH)
                {
                    best = next;
                    bestH = currentH;
                }
            }
            else
            {
                double probability = Math.Exp(-deltaE / T);
                if (new Random().NextDouble() < probability)
                {
                    current = next;
                    currentH = nextH;
                }
            }
            
            t++;
        }

        result.Success = false;
        result.Solution = best;
        result.TimeElapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
        return result;
    }
}
