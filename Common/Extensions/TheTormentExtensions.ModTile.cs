using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheTorment.Common.Extensions;

public static partial class TheTormentExtensions {
    public static void DefaultToSolid(this ModTile modTile, 
                                           bool mergeDirt = true,
                                           bool lighted = true, 
                                           bool blockLight = true, 
                                           bool blendAll = true) {
        ushort type = modTile.Type;

        Main.tileSolid[type] = true;

        Main.tileMergeDirt[type] = mergeDirt;

        Main.tileBlockLight[type] = blockLight;
        Main.tileLighted[type] = lighted;

        Main.tileBlendAll[type] = blendAll;
    }

    public static void DefaultToGrass(this ModTile modTile,
                                           ushort tile) {
        modTile.DefaultToSolid();

        ushort type = modTile.Type;

        TileID.Sets.Grass[type] = true;
        TileID.Sets.NeedsGrassFraming[type] = true;
        TileID.Sets.NeedsGrassFramingDirt[type] = tile;

        TileID.Sets.Conversion.Grass[type] = true;
    }

    public static void TileBlend(this ModTile modTile,
                                      ushort tile) {
        ushort type = modTile.Type;

        Main.tileMerge[type][tile] = true;
        Main.tileMerge[tile][type] = true;
    }
}