using Terraria;

namespace TheTorment.Common.Utilities;

public static partial class TheTormentUtilities {
    public static Tile SafeGetTile(int x,
                                   int y)
        => !WorldGen.InWorld(x, y) ?
           Framing.GetTileSafely(1, 1) :
           Framing.GetTileSafely(x, y);
}