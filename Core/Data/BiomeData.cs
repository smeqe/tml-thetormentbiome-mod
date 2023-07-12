using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace TheTorment.Core.Data;

public record struct BiomeData(byte ID,
                               int X,
                               int Y,
                               int Width,
                               int Height,
                               HashSet<(ushort, Point)> TilesToDestroy,
                               HashSet<(ushort, Point)> TilesToReplace) {
    public Point Position
        => new(X, Y);

    public Point Center
        => new(X + Width / 2, Y + Height / 2);
}