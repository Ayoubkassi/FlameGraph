namespace FlameGraph;

// GIVEN: Deterministic color — same function always gets the same color.
// Classic flamegraph "hot" palette: reds, oranges, yellows.

public static class FlameColors
{
    public static string GetColor(string name)
    {
        uint hash = 2166136261;
        foreach (char c in name) { hash ^= c; hash *= 16777619; }

        double h = hash % 60;
        double s = 65 + (hash >> 4) % 25;
        double l = 55 + (hash >> 8) % 15;

        return HslToHex(h, s, l);
    }

    private static string HslToHex(double h, double s, double l)
    {
        h /= 360; s /= 100; l /= 100;
        double r, g, b;

        if (s == 0) { r = g = b = l; }
        else
        {
            double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            double p = 2 * l - q;
            r = HueToRgb(p, q, h + 1.0 / 3);
            g = HueToRgb(p, q, h);
            b = HueToRgb(p, q, h - 1.0 / 3);
        }

        return $"#{(int)Math.Round(r * 255):X2}{(int)Math.Round(g * 255):X2}{(int)Math.Round(b * 255):X2}";
    }

    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2) return q;
        if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
        return p;
    }
}
