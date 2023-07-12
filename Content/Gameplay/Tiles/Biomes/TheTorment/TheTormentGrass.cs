using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using TheTorment.Common.Extensions;

namespace TheTorment.Content.Gameplay.Tiles.Biomes.TheTorment;

public sealed class TheTormentGrassDirt : ModTile {
    internal static Color MapColor = new(69, 138, 173);

    public override string Texture
        => base.Texture.TheTormentTexturePath().Replace("Dirt", string.Empty);

    public override void SetStaticDefaults() {
        this.DefaultToGrass(TileID.Dirt);

        this.TileBlend((ushort)ModContent.TileType<TheTormentStone>());
        this.TileBlend((ushort)ModContent.TileType<TheTormentSand>());

        AddMapEntry(MapColor);
        //SetModTree(new ReachTree());
    }
}

public sealed class TheTormentGrassMud : ModTile {
    public override string Texture
        => base.Texture.TheTormentTexturePath();

    public override void SetStaticDefaults() {
        this.DefaultToGrass(TileID.Mud);

        this.TileBlend((ushort)ModContent.TileType<TheTormentStone>());
        this.TileBlend((ushort)ModContent.TileType<TheTormentSand>());

        AddMapEntry(TheTormentGrassDirt.MapColor);
    }
}