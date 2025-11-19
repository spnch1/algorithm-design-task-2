namespace AlgorithmDesignTask2;

public static class Heuristics
{
    public static int F2(State state)
    {
        int conflicts = 0;
        int n = state.N;
        int[] q = state.Queens;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (q[i] == q[j])
                {
                    conflicts++;
                    continue;
                }

                int dx = Math.Abs(i - j);
                int dy = Math.Abs(q[i] - q[j]);
                if (dx == dy)
                {
                    conflicts++;
                }
            }
        }
        return conflicts;
    }

    public static int Custom(State state)
    {
        int threatenedQueens = 0;
        int n = state.N;
        int[] q = state.Queens;

        for (int i = 0; i < n; i++)
        {
            bool isThreatened = false;
            for (int j = 0; j < n; j++)
            {
                if (i == j) continue;

                if (q[i] == q[j])
                {
                    isThreatened = true;
                    break;
                }

                int dx = Math.Abs(i - j);
                int dy = Math.Abs(q[i] - q[j]);
                if (dx == dy)
                {
                    isThreatened = true;
                    break;
                }
            }
            if (isThreatened)
            {
                threatenedQueens++;
            }
        }
        return threatenedQueens;
    }
}
