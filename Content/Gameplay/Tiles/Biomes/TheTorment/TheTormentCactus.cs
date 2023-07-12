using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

using TheTorment.Graphics;

namespace TheTorment.Content.Gameplay.Tiles.Biomes.TheTorment;

public sealed class TheTormentCactus : ModCactus {
	public override void SetStaticDefaults()
		=> GrowsOnTileId = new int[1] { ModContent.TileType<TheTormentSand>() };

	public override Asset<Texture2D> GetTexture()
		=> ModContent.Request<Texture2D>($"{TexturePaths.PATHTO_TEXTURES_TILES_THETORMENT}/{nameof(TheTormentCactus)}");

	public override Asset<Texture2D> GetFruitTexture()
		=> null;
}