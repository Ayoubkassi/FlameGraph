namespace FlameGraph;

// GIVEN: Simulates a CPU profiler capturing stack samples.
// This is provided — participants don't need to write this.

public static class SampleGenerator
{
    public static List<string[]> Generate(int count, int seed = 42)
    {
        var rng = new Random(seed);
        var samples = new List<string[]>();

        for (int i = 0; i < count; i++)
        {
            var stack = new List<string> { "Main", "ProcessRequests", "HandleRequest" };

            if (rng.NextDouble() < 0.80)
                stack.Add("ValidateInput");

            stack.Add("LookupUser");

            if (rng.NextDouble() < 0.60)
            {
                stack.Add("CacheGet");
            }
            else
            {
                stack.Add("DatabaseQuery");
                if (rng.NextDouble() < 0.50)
                    stack.Add("ParseRow");
            }

            if (rng.NextDouble() < 0.70)
            {
                stack.Add("CalculatePrice");
                stack.Add("GetTaxRate");
                if (rng.NextDouble() < 0.30)
                    stack.Add("ApplyDiscount");
            }

            if (rng.NextDouble() < 0.90)
                stack.Add("SerializeResponse");

            if (rng.NextDouble() < 0.80)
                stack.Add("LogRequest");

            samples.Add(stack.ToArray());
        }

        return samples;
    }
}
