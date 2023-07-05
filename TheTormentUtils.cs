using Terraria;

namespace TheTorment;

public static partial class TheTormentUtils {
    public static Tile SafeGetTile(int x, 
                                   int y) 
        => !WorldGen.InWorld(x, y) ? Framing.GetTileSafely(1, 1) : Framing.GetTileSafely(x, y);
}