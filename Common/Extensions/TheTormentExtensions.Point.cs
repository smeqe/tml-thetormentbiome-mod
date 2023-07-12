using Microsoft.Xna.Framework;

using System;

using Terraria;

namespace TheTorment.Common.Extensions;

public static partial class TheTormentExtensions {
    public static bool IsInWorld(this Point point) 
        => point.X >= 0 && point.Y >= 0 && point.X < Main.maxTilesX && point.Y < Main.maxTilesY;

    public static int Distance(this Point point1,
                                    Point point2) {
        int x = point1.X - point2.X;
        int y = point1.Y - point2.Y;
        return (int)Math.Sqrt(x * x + y * y);
    }
}