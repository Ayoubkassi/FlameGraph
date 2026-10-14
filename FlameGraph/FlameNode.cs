namespace FlameGraph;

// Step 1: The tree node — represents one function in the call tree.
// Each node knows its sample count and can have children.

public sealed class FlameNode(string name)
{
    public string Name { get; } = name;
    public int Samples { get; set; }
    public int Width { get; set; }

    // Computed layout coordinates (pixels)
    public int X { get; set; }
    public int Y { get; set; }
    public int PixelWidth { get; set; }

    // Children keyed by function name (for fast lookup when building)
    public Dictionary<string, FlameNode> Children { get; } = new();

    // Sorted children for rendering
    public List<FlameNode> SortedChildren { get; set; } = new();

    public FlameNode GetOrAddChild(string childName)
    {
        if (!Children.TryGetValue(childName, out var child))
        {
            child = new FlameNode(childName);
            Children.Add(childName, child);
        }
        return child;
    }
}
