using System.Globalization;
using System.Security;
using System.Text;

namespace FlameGraph;

// GIVEN: Low-level SVG writer — wraps rectangles, text, etc.
// Participants use this to render; they don't need to write it.

public sealed class SvgWriter(int width, int height)
{
    private readonly StringBuilder _sb = new();

    public void Begin()
    {
        _sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\">");
        _sb.AppendLine("<rect width=\"100%\" height=\"100%\" fill=\"#ffffff\"/>");
    }

    public void End()
    {
        _sb.AppendLine("</svg>");
    }

    public void DrawRect(
        int x, int y, double width, int height,
        string color, string title = "")
    {
        string w = width.ToString("F1", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(title))
            _sb.AppendLine($"<g><title>{SecurityElement.Escape(title)}</title>");

        _sb.AppendLine($"<rect x=\"{x}\" y=\"{y}\" width=\"{w}\" height=\"{height}\" fill=\"{color}\" stroke=\"#ffffff\" stroke-width=\"0.5\"/>");

        if (!string.IsNullOrEmpty(title))
            _sb.AppendLine("</g>");
    }

    public void DrawText(int x, int y, string text)
    {
        string escaped = SecurityElement.Escape(text)!;
        _sb.AppendLine($"<text x=\"{x}\" y=\"{y}\" font-family=\"Arial,sans-serif\" font-size=\"11\" fill=\"#222222\">{escaped}</text>");
    }

    public void Save(string filename)
    {
        File.WriteAllText(filename, _sb.ToString());
    }
}
