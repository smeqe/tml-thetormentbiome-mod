using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace TheTorment.Common.Utilities;

public static partial class TheTormentUtilities {
    public static double RockLayerMid
        => GenVars.rockLayerLow + (GenVars.rockLayerHigh - GenVars.rockLayerLow) / 2.0;

    public static float WorldSize 
        => Main.maxTilesX / 4200f;

    public static void ReplaceTile(int i, 
                                   int j, 
                                   int t,
                                   bool item = false, 
                                   bool mute = true, 
                                   int style = 0) {
        WorldGen.KillTile(i, j, false, false, item);
        WorldGen.PlaceTile(i, j, t, true, mute, -1, style);
    }

    public static void ReplaceTile(Point position, 
                                   int type, 
                                   bool item = false, 
                                   bool mute = true, 
                                   int style = 0)
        => ReplaceTile(position.X, position.Y, type, item, mute, style);

    public static void ReplaceTile(Point16 position, 
                                   int type, 
                                   bool item = false, 
                                   bool mute = true, 
                                   int style = 0)
        => ReplaceTile(position.X, position.Y, type, item, mute, style);

    public static void ReplaceWall(Point16 position, 
                                   int type, 
                                   bool mute = true) {
        WorldGen.KillWall(position.X, position.Y, false);
        WorldGen.PlaceWall(position.X, position.Y, type, mute);
    }

    public static void ReplaceWall(int x,
                                   int y, 
                                   int type, 
                                   bool mute = true) 
        => ReplaceWall(new Point(x, y), type, mute);

    public static void ReplaceWall(Point position, 
                                   int type, 
                                   bool mute = true)
        => ReplaceWall(new Point16(position.X, position.Y), type, mute);
}