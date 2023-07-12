using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using TheTorment.Common.Extensions;

namespace TheTorment.Content.Gameplay.Tiles.Biomes.TheTorment;

public sealed class TheTormentStone : ModTile {
    public override string Texture
        => base.Texture.TheTormentTexturePath();

    public override void SetStaticDefaults() {
        this.DefaultToSolid();

        Main.tileStone[Type] = true;

        this.TileBlend((ushort)ModContent.TileType<TheTormentGrassDirt>());
        this.TileBlend((ushort)ModContent.TileType<TheTormentGrassMud>());

        HitSound = SoundID.Tink;

        DustType = DustID.Stone;

        //RegisterItemDrop(ItemType<PalestoneItem>());

        AddMapEntry(new Color(46, 73, 94));
    }
}