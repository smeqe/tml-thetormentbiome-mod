using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using TheTorment.Common.Extensions;

namespace TheTorment.Content.Gameplay.Tiles.Biomes.TheTorment;

public sealed class TheTormentSand : ModTile {
    public override string Texture
        => base.Texture.TheTormentTexturePath();
    
    public override void SetStaticDefaults() {
        this.DefaultToSolid();

        Main.tileStone[Type] = true; //don't ask me about anything but this is what makes both tileMergeDirt and blendAll work altogether

        TileID.Sets.Conversion.Sand[Type] = true;

        this.TileBlend((ushort)ModContent.TileType<TheTormentStone>());

        AddMapEntry(new Color(72, 94, 109));

		//DustType = DustID.Water_Space;
	}

    //public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    //{
    //    if (WorldGen.noTileActions)
    //        return true;

    //    Tile above = Main.tile[i, j - 1];
    //    Tile below = Main.tile[i, j + 1];
    //    bool canFall = true;

    //    if (below == null || below.active())
    //        canFall = false;

    //    if (above.active() && (TileID.Sets.BasicChest[above.type] || TileID.Sets.BasicChestFake[above.type] || above.type == TileID.PalmTree || TileLoader.IsDresser(above.type)))
    //        canFall = false;

    //    if (canFall)
    //    {
    //        //Set the projectile type to ExampleSandProjectile
    //        int projectileType = ModContent.ProjectileType<SpiritSand>();
    //        float positionX = i * 16 + 8;
    //        float positionY = j * 16 + 8;

    //        if (Main.netMode == NetmodeID.SinglePlayer)
    //        {
    //            Main.tile[i, j].ClearTile();
    //            int proj = Projectile.NewProjectile(positionX, positionY, 0f, 0.41f, projectileType, 10, 0f, Main.myPlayer);
    //            Main.projectile[proj].ai[0] = 1f;
    //            WorldGen.SquareTileFrame(i, j);
    //        }
    //        else if (Main.netMode == NetmodeID.Server)
    //        {
    //            Main.tile[i, j].active(false);
    //            bool spawnProj = true;

    //            for (int k = 0; k < 1000; k++)
    //            {
    //                Projectile otherProj = Main.projectile[k];

    //                if (otherProj.active && otherProj.owner == Main.myPlayer && otherProj.type == projectileType && Math.Abs(otherProj.timeLeft - 3600) < 60 && otherProj.Distance(new Vector2(positionX, positionY)) < 4f)
    //                {
    //                    spawnProj = false;
    //                    break;
    //                }
    //            }

    //            if (spawnProj)
    //            {
    //                int proj = Projectile.NewProjectile(positionX, positionY, 0f, 2.5f, projectileType, 10, 0f, Main.myPlayer);
    //                Main.projectile[proj].velocity.Y = 0.5f;
    //                Main.projectile[proj].position.Y += 2f;
    //                Main.projectile[proj].netUpdate = true;
    //            }

    //            NetMessage.SendTileSquare(-1, i, j, 1);
    //            WorldGen.SquareTileFrame(i, j);
    //        }
    //        return false;
    //    }
    //    return true;
    //}
}

