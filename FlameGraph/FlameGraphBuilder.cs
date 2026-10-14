namespace FlameGraph;

public sealed class FlameGraphBuilder
{
    private readonly int _sampleWidth;
    private readonly int _frameHeight;

    public FlameGraphBuilder(int sampleWidth = 5, int frameHeight = 22)
    {
        _sampleWidth = sampleWidth;
        _frameHeight = frameHeight;
    }

    public FlameNode BuildTree(List<string[]> samples)
    {
        var root = new FlameNode("(root)");

        foreach (var sample in samples)
        {
            var current = root;
            current.Samples++;

            foreach (string func in sample)
            {
                current = current.GetOrAddChild(func);
                current.Samples++;
            }
        }

        return root;
    }

    public void CalculateWidths(FlameNode node)
    {
        node.Width = node.Samples;

        foreach (var child in node.Children.Values)
            CalculateWidths(child);
    }

    public void SortChildren(FlameNode node)
    {
        node.SortedChildren = node.Children.Values
            .OrderBy(c => c.Name, StringComparer.Ordinal)
            .ToList();

        foreach (var child in node.SortedChildren)
            SortChildren(child);
    }

    public void AssignCoordinates(FlameNode node, int x, int depth, int svgHeight)
    {
        node.X = x * _sampleWidth;
        node.Y = svgHeight - (depth + 1) * _frameHeight;
        node.PixelWidth = node.Width * _sampleWidth;

        int childX = x;

        foreach (var child in node.SortedChildren)
        {
            AssignCoordinates(child, childX, depth + 1, svgHeight);
            childX += child.Width;
        }
    }

    public void Render(FlameNode root, string filename)
    {
        int maxDepth = GetMaxDepth(root);
        int svgWidth = (root.Samples - 1) * _sampleWidth;
        int svgHeight = (maxDepth + 1) * _frameHeight + 10;

        AssignCoordinates(root, x: 0, depth: -1, svgHeight);

        var svg = new SvgWriter(svgWidth, svgHeight);
        svg.Begin();

        RenderNode(svg, root);

        svg.End();
        svg.Save(filename);
    }

    private void RenderNode(SvgWriter svg, FlameNode node)
    {
        if (node.Name != "(root)")
        {
            string color = FlameColors.GetColor(node.Name);

            svg.DrawRect(
                node.X, node.Y,
                node.PixelWidth - 1, _frameHeight - 1,
                color,
                $"{node.Name}(): {node.Samples} samples");

            if (node.PixelWidth >= 100)
                svg.DrawText(node.X + 4, node.Y + 15, node.Name + "()");
        }

        foreach (var child in node.SortedChildren)
            RenderNode(svg, child);
    }

    public int GetMaxDepth(FlameNode node, int depth = -1)
    {
        if (node.SortedChildren.Count == 0)
            return depth;
        return node.SortedChildren.Max(c => GetMaxDepth(c, depth + 1));
    }
}
