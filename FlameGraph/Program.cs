using FlameGraph;

// ============================================================
// CODING DOJO: Build a Flame Graph
// ============================================================
//
// We go through 8 steps together:
//
// 1. Look at the data (stack samples)
// 2. Draw raw samples as SVG (no merging)
// 3. Build a call tree from samples
// 4. Calculate widths
// 5. Sort children alphabetically
// 6. Assign coordinates
// 7. Render the flame graph
// 8. Open in browser — compare step 1 vs step 7
//
// ============================================================

const int SampleCount = 300;
const int FrameHeight = 22;
const int PixelsPerSample = 5;

Console.WriteLine("=== Coding Dojo: Flame Graph ===\n");

// ============================================================
// Step 1: Look at the data
// ============================================================

Console.WriteLine("Step 1: Generate stack samples\n");

var samples = SampleGenerator.Generate(SampleCount);

Console.WriteLine($"  Total samples:   {samples.Count}");
Console.WriteLine($"  Min depth:       {samples.Min(s => s.Length)}");
Console.WriteLine($"  Max depth:       {samples.Max(s => s.Length)}");
Console.WriteLine($"  Distinct stacks: {samples.Select(s => string.Join(";", s)).Distinct().Count()}");
Console.WriteLine();
Console.WriteLine("  Example stack:");
Console.WriteLine($"    {string.Join(" -> ", samples[0])}");
Console.WriteLine();

// ============================================================
// Step 2: Draw raw samples as SVG (no merging)
// ============================================================

Console.WriteLine("Step 2: Raw samples -> step1_raw.svg\n");
RawSampleRenderer.Render(samples, "step1_raw.svg", PixelsPerSample, FrameHeight);

// ============================================================
// Steps 3-7: Build the flame graph
// ============================================================

var builder = new FlameGraphBuilder(PixelsPerSample, FrameHeight);

// Step 3: Build call tree
Console.WriteLine("Step 3: Build call tree");
var root = builder.BuildTree(samples);
Console.WriteLine($"  Root samples: {root.Samples}");
Console.WriteLine($"  Top-level functions: {root.Children.Count}\n");

// Step 4: Calculate widths
Console.WriteLine("Step 4: Calculate widths");
builder.CalculateWidths(root);
Console.WriteLine($"  Root width: {root.Width}\n");

// Step 5: Sort children
Console.WriteLine("Step 5: Sort children alphabetically\n");
builder.SortChildren(root);

// Step 6: Assign coordinates
Console.WriteLine("Step 6: Assign coordinates\n");
int maxDepth = builder.GetMaxDepth(root);
Console.WriteLine($"  Max depth: {maxDepth}\n");

// Step 7: Render
Console.WriteLine("Step 7: Render -> step2_flamegraph.svg\n");
builder.Render(root, "step2_flamegraph.svg");

// ============================================================
// Step 8: Done!
// ============================================================

Console.WriteLine("Step 8: Done! Open SVGs in browser.\n");

// ============================================================
// GIVEN: Raw sample rendering moved to RawSampleRenderer.cs
// ============================================================
