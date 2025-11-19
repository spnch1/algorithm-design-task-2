namespace AlgorithmDesignTask2;

public class SearchResult
{
    public bool Success { get; set; }
    public State? Solution { get; set; }
    public int Steps { get; set; }
    public int GeneratedStates { get; set; }
    public int MaxMemoryStates { get; set; }
    public int DeadEnds { get; set; }
    public double TimeElapsedSeconds { get; set; }
}

public interface ISolver
{
    SearchResult Solve(State initialState, Func<State, int> heuristic);
}
