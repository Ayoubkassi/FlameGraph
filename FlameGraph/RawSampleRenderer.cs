namespace FlameGraph;

// GIVEN: Renders raw samples as SVG columns (no merging).
// This is Step 2 — the "before" picture.

public static class RawSampleRenderer
{
    public static void Render(List<string[]> samples, string filename, int sampleWidth = 5, int frameHeight = 22)
    {
        int maxDepth = samples.Max(s => s.Length);
        int svgWidth = samples.Count * sampleWidth;
        int svgHeight = maxDepth * frameHeight + 10;

        var svg = new SvgWriter(svgWidth, svgHeight);
        svg.Begin();

        for (int i = 0; i < samples.Count; i++)
        {
            var stack = samples[i];
            int x = i * sampleWidth;

            for (int depth = 0; depth < stack.Length; depth++)
            {
                string func = stack[depth];
                int y = svgHeight - (depth + 1) * frameHeight;
                string color = FlameColors.GetColor(func);

                svg.DrawRect(x, y, sampleWidth - 0.5, frameHeight - 1, color, $"{func}() - sample #{i}");
            }
        }

        svg.End();
        svg.Save(filename);
    }
}
