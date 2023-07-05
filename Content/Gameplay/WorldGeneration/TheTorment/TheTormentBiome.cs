using ReLogic.Utilities;
using System;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheTorment.Content.Gameplay.WorldGeneration.TheTorment;

public sealed partial class TheTormentBiome : ModSystem {
    internal class TheTormentBiomePass : GenPass {
        internal TheTormentBiomePass(string name,
                                     float loadWeight) 
            : base(name,
                   loadWeight) {
        }

        protected override void ApplyPass(GenerationProgress progress, 
                                          GameConfiguration configuration) {
            double num = _random.Next(80, 100) * 2;
            double num2 = _random.Next(20, 26);
            double num3 = (double)Main.maxTilesX / 4200.0;

            num *= num3;
            num2 *= num3;
            double num4 = num2 - 1.0;
            double num5 = num;
            Vector2D vector2D = default(Vector2D);
            vector2D.X = Main.spawnTileX;
            vector2D.Y = 200;
            while (!WorldGen.SolidTile((int)vector2D.X, (int)vector2D.Y)) {
                vector2D.Y++;
            }
            Vector2D vector2D2 = default(Vector2D);
            vector2D2.X = (double)_random.Next(-100, 101) * 0.005;
            vector2D2.Y = (double)_random.Next(-200, -100) * 0.005;
            while (num > 0.0 && num2 > 0.0)
            {
                num -= (double)_random.Next(3);
                num2 -= 1.0;
                int num6 = (int)(vector2D.X - num * 0.5);
                int num7 = (int)(vector2D.X + num * 0.5);
                int num8 = (int)(vector2D.Y - num * 0.5);
                int num9 = (int)(vector2D.Y + num * 0.5);
                if (num6 < 0)
                    num6 = 0;

                if (num7 > Main.maxTilesX)
                    num7 = Main.maxTilesX;

                if (num8 < 0)
                    num8 = 0;

                if (num9 > Main.maxTilesY)
                    num9 = Main.maxTilesY;

                num5 = num * (double)_random.Next(80, 120) * 0.01;
                for (int k = num6; k < num7; k++)
                {
                    for (int l = num8; l < num9; l++)
                    {
                        double num10 = Math.Abs((double)k - vector2D.X);
                        double num11 = Math.Abs((double)l - vector2D.Y);
                        double num12 = Math.Sqrt(num10 * num11 + num10 * num11);
                        if (num12 < num5 * 0.4 * (0.95 + _random.NextDouble() * 0.1))
                        {
                            if (l < vector2D.Y + 10)
                            {
                                Main.tile[k, l].TileType = TileID.IronBrick;
                            }
                            else
                            {
                                WorldGen.KillTile(k, l);
                                WorldGen.PlaceTile(k, l, TileID.IronBrick);
                            }

                            if (Main.tile[k, l].WallType > 0)
                                Main.tile[k, l].WallType = 80;
                        }
                    }
                }
            }
        }
    }
}
