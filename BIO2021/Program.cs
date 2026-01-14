using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main()
    {
        // Example usage based on your structure
        // Param 1: Target for 3(a) (e.g., "ACBD")
        // Param 2: Unused for this specific problem logic, but kept for constructor signature
        WindowSolver ws = new WindowSolver("ACBD", "");

        ws.Calculate();

        // 3(a) Output
        Console.WriteLine($"3(a) Min Ops for ACBD: {ws.First}");
        
        // 3(b) Output
        Console.WriteLine($"3(b) Orderings of 5 boxes with 6 ops: {ws.Second}");
        
        // 3(c) Output
        Console.WriteLine($"3(c) Paths to HGFEDCBA: {ws.Third}");

        // Verifying the sample provided in the prompt
        Debug.Assert(ws.First == "6");
    }
}

public class WindowSolver
{
    // Input for Part 3(a)
    string targetA;

    // Outputs
    string answer1, answer2, answer3;

    public String First => answer1;
    public String Second => answer2;
    public String Third => answer3;

    public WindowSolver(String s1, String s2)
    {
        targetA = s1;
        // s2 is not needed for this specific logic but kept to match your 
    }

    public void Calculate()
    {
        // --- Part 3(a): Minimum operations for input string ---
        answer1 = BfsMinSteps(targetA).ToString();

        // --- Part 3(b): Orderings of 5 boxes taking exactly 6 operations ---
        answer2 = BfsFindStates(targetLen: 5, targetDist: 6);

        // --- Part 3(c): Count paths to HGFEDCBA ---
        answer3 = BfsCountPaths("HGFEDCBA").ToString();
    }

    // Logic for 3(a): Basic BFS to find shortest distance
    private int BfsMinSteps(string target)
    {
        if (string.IsNullOrEmpty(target)) return 0;

        Queue<(string state, int dist)> queue = new Queue<(string, int)>();
        HashSet<string> visited = new HashSet<string>();

        queue.Enqueue(("", 0)); // Start empty
        visited.Add("");

        int maxLen = target.Length;

        while (queue.Count > 0)
        {
            var (curr, dist) = queue.Dequeue();

            if (curr == target) return dist;

            // Try all 3 operations
            foreach (var next in GetNextStates(curr, maxLen))
            {
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue((next, dist + 1));
                }
            }
        }
        return -1; // Should not happen
    }

    // Logic for 3(b): Find all states of specific length and specific distance
    private string BfsFindStates(int targetLen, int targetDist)
    {
        Queue<(string state, int dist)> queue = new Queue<(string, int)>();
        Dictionary<string, int> visited = new Dictionary<string, int>();
        List<string> foundStates = new List<string>();

        queue.Enqueue(("", 0));
        visited[""] = 0;

        while (queue.Count > 0)
        {
            var (curr, dist) = queue.Dequeue();

            // Optimization: If we went past the target distance, we can stop this branch
            if (dist > targetDist) continue;

            // Check if this state matches our criteria
            if (curr.Length == targetLen && dist == targetDist)
            {
                foundStates.Add(curr);
            }

            // Only expand if we haven't reached the max length yet
            // or if we need to rearrange existing items (Swap/Rotate)
            foreach (var next in GetNextStates(curr, targetLen))
            {
                // Standard BFS visited check
                if (!visited.ContainsKey(next))
                {
                    visited[next] = dist + 1;
                    queue.Enqueue((next, dist + 1));
                }
            }
        }

        // Sort results for clean output
        foundStates.Sort();
        return string.Join(", ", foundStates);
    }

    // Logic for 3(c): Count number of shortest paths to specific target
    private long BfsCountPaths(string target)
    {
        int maxLen = target.Length;
        
        Queue<string> queue = new Queue<string>();
        
        // Dictionary to store minimum distance to reach a state
        Dictionary<string, int> dist = new Dictionary<string, int>();
        
        // Dictionary to store number of ways to reach that state at that distance
        Dictionary<string, long> paths = new Dictionary<string, long>();

        queue.Enqueue("");
        dist[""] = 0;
        paths[""] = 1; // Base case: 1 way to be empty (start)

        while (queue.Count > 0)
        {
            string u = queue.Dequeue();
            int currentDist = dist[u];
            long currentPaths = paths[u];

            // If we have reached the target's distance, we don't need to expand further
            // (Expanding would increase distance, making it not a shortest path)
            if (u == target) continue; 

            foreach (var v in GetNextStates(u, maxLen))
            {
                // Case 1: First time discovering state v
                if (!dist.ContainsKey(v))
                {
                    dist[v] = currentDist + 1;
                    paths[v] = currentPaths; // Inherit path count
                    queue.Enqueue(v);
                }
                // Case 2: Found state v again via a different path of the SAME length
                else if (dist[v] == currentDist + 1)
                {
                    paths[v] += currentPaths; // Add new paths to existing count
                }
            }
        }

        if (paths.ContainsKey(target))
        {
            return paths[target];
        }
        return 0;
    }

    // --- Helper: The 3 Operations ---
    private IEnumerable<string> GetNextStates(string s, int maxLen)
    {
        int len = s.Length;

        // 1. ADD: Add next alphabetical char if below max length
        // Logic: If len is 0, add 'A' (char 65). If len is 1, add 'B' (char 66).
        if (len < maxLen)
        {
            char nextChar = (char)('A' + len);
            yield return s + nextChar;
        }

        // Operations below require at least 2 boxes
        if (len >= 2)
        {
            // 2. SWAP: Swap first two elements
            // Strings are immutable, so we use char array or substring
            char c1 = s[0];
            char c2 = s[1];
            string remainder = s.Substring(2);
            yield return c2.ToString() + c1.ToString() + remainder;

            // 3. ROTATE: Move first element to end
            yield return s.Substring(1) + s[0];
        }
    }
}