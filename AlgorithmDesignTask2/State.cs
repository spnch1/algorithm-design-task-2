namespace AlgorithmDesignTask2;

public class State : IEquatable<State>
{
    public int[] Queens { get; } // index = column; value = row
    public int N { get; }

    public State(int n)
    {
        N = n;
        Queens = new int[n];
        var rnd = new Random();
        for (int i = 0; i < n; i++)
        {
            Queens[i] = rnd.Next(n);
        }
    }

    public State(int[] queens)
    {
        N = queens.Length;
        Queens = (int[])queens.Clone();
    }

    public List<State> GetNeighbors()
    {
        var neighbors = new List<State>();
        for (int col = 0; col < N; col++)
        {
            int currentRow = Queens[col];
            for (int row = 0; row < N; row++)
            {
                if (row == currentRow) continue;

                var newQueens = (int[])Queens.Clone();
                newQueens[col] = row;
                neighbors.Add(new State(newQueens));
            }
        }
        return neighbors;
    }
    
    public State GetRandomNeighbor()
    {
        var rnd = new Random();
        int col = rnd.Next(N);
        int currentRow = Queens[col];
        int newRow;
        do
        {
            newRow = rnd.Next(N);
        } while (newRow == currentRow);

        var newQueens = (int[])Queens.Clone();
        newQueens[col] = newRow;
        return new State(newQueens);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as State);
    }

    public bool Equals(State? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Queens.SequenceEqual(other.Queens);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var q in Queens)
        {
            hash = hash * 31 + q;
        }
        return hash;
    }

    public override string ToString()
    {
        return string.Join(",", Queens);
    }
    
    public void PrintBoard()
    {
        Console.WriteLine("   " + string.Join(" ", Enumerable.Range(0, N).Select(i => i.ToString())));
        for (int r = 0; r < N; r++)
        {
            Console.Write($"{r,2} ");
            for (int c = 0; c < N; c++)
            {
                if ((r + c) % 2 == 0) Console.BackgroundColor = ConsoleColor.DarkGray;
                else Console.BackgroundColor = ConsoleColor.Black;

                if (Queens[c] == r)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("QQ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("  ");
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
