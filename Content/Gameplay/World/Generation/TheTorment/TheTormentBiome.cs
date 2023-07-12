using Microsoft.Xna.Framework;

using ReLogic.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

using TheTorment.Common.Utilities;
using TheTorment.Content.Gameplay.Tiles.Biomes.TheTorment;
using TheTorment.Core.Data;

namespace TheTorment.Content.Gameplay.World.Generation.TheTorment;

public sealed partial class TheTormentBiome : ModSystem {
    internal class TheTormentBiomePassCleanUp : GenPass {
        internal TheTormentBiomePassCleanUp(string name,
            float loadWeight) 
            : base(name,
                   loadWeight) {
        }

        protected override void ApplyPass(GenerationProgress progress, 
                                          GameConfiguration configuration) {
            foreach (BiomeData biome in TheTormentBiomePass._biomes) {
                for (int index = 0; index < 2; index++) { 
                    for (int x = biome.X - 50; x < biome.X + biome.Width + 50; x++) {
                        for (int y = 200; y < (int)(Main.worldSurface + 40.0) - 30; y++) {
                            Tile tile = TheTormentUtilities.SafeGetTile(x, y);
                            ushort tileType = tile.TileType;
                            int[] plants = new int[] { TileID.Plants, TileID.Plants2, TileID.Vines, TileID.VineFlowers };
                            if (plants.Contains(tileType)) {
                                WorldGen.KillTile(x, y);
                            }
                            else if (TileID.Sets.Grass[tileType] && tileType != TileID.JungleGrass && tile.WallType != WallID.FlowerUnsafe && tile.WallType != 68) {
                                tile.TileType = TheTormentBiomePass._tileID_Grass1;
                            }
                            else if (tileType == TileID.Sand) {
                                tile.TileType = TheTormentBiomePass._tileID_Sand;
                            }
                            else if (y < (int)(Main.worldSurface - 20.0) && tileType == TileID.Stone) {
                                tile.TileType = TheTormentBiomePass._tileID_Stone;
                            }
                        } 
                    }
                }
            }
        }
    }

    internal class TheTormentBiomePass : GenPass {
        internal const int WIDTH = 300;
        internal const int HEIGHT = 200;

        private GenerationProgress? _progress;

        private List<Point> _centerTiles;

        private Point _tunnelStartPosition;

        internal static ushort _tileID_Stone,
                               _tileID_Grass1,
                               _tileID_Grass2,
                               _tileID_Sand;

        internal static BiomeData[] _biomes;

        private static int[] InvalidTiles
            => new int[] { TileID.Copper, TileID.Tungsten, TileID.Gold, TileID.Platinum, TileID.Silver, TileID.Tin, TileID.HardenedSand, TileID.Sandstone };

        private static int[] InvalidWalls
            => new int[] { WallID.Granite, WallID.GraniteUnsafe, WallID.Marble, WallID.MarbleUnsafe, WallID.SandstoneEcho, WallID.SandFall, WallID.HardenedSandEcho, WallID.HardenedSand, WallID.Sandstone };

        private static int[] InvalidTilesToDestroy
            => new int[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick };

        internal TheTormentBiomePass(string name,
                                     float loadWeight)
            : base(name,
                   loadWeight) {
            _biomes = new BiomeData[(int)((double)Main.maxTilesX * 0.00045)];
        }

        protected override void ApplyPass(GenerationProgress progress,
                                          GameConfiguration configuration) {
            _progress = progress;

            Step0_Setup();
            Step1_FindPosition();
            Step2_MakeShape();
            Step3_BasePlacement();
            Step4_SpreadGrass();
            Step5_CaveEntrance();
            Step6_FirstTunnel();

            //foreach (Point pos in _centerTiles)
            //    TheTormentUtilities.ReplaceTile(pos, TileID.GoldBrick);
        }

        private void Step0_Setup() {
            AnnounceProgress("Setuping", 0f);

            void SetupMain() {
                _centerTiles = new List<Point>();

                for (int k = 0; k < _biomes?.Length; k++) {
                    _biomes[k] = new BiomeData() {
                        ID = (byte)k,
                        Width = (int)(TheTormentUtilities.WorldSize * WIDTH),
                        Height = (int)(TheTormentUtilities.WorldSize * HEIGHT),
                        TilesToDestroy = new HashSet<(ushort, Point)>(),
                        TilesToReplace = new HashSet<(ushort, Point)>()
                    };
                }
            }

            void SetupTileIDs() {
                _tileID_Stone = (ushort)ModContent.TileType<TheTormentStone>();
                _tileID_Grass1 = (ushort)ModContent.TileType<TheTormentGrassDirt>();
                _tileID_Grass2 = (ushort)ModContent.TileType<TheTormentGrassMud>();
                _tileID_Sand = (ushort)ModContent.TileType<TheTormentSand>();
            }

            SetupMain();
            SetupTileIDs();
        }

        private void Step1_FindPosition() {
            AnnounceProgress("FindingPosition", 0.1f);

            void SetXForEachBiome() {
                foreach (BiomeData biome in _biomes) {
                    int num778 = _worldWidth;
			        int num779 = 0;
			        int num780 = _worldWidth;
			        int num781 = 0;
			        for (int i = 0; i < _worldWidth; i++) {
				        for (int j = 0; (double)j < Main.worldSurface; j++) {
                            Tile tile = Main.tile[i, j];
                            if (tile.HasTile) {
                                if (tile.TileType == TileID.JungleGrass || 
                                    tile.TileType == TileID.Mud) {
                                    if (i < num778) {
                                        num778 = i;
                                    }
                                    if (i > num779) {
                                        num779 = i;
                                    }
                                }
                                else if (tile.TileType == TileID.SnowBlock ||
                                         tile.TileType == TileID.IceBlock) {
                                    if (i < num780) {
                                        num780 = i;
                                    }
                                    if (i > num781) {
                                        num781 = i;
                                    }
						        }
					        }
				        }
			        }
			        int num784 = 10;
			        num778 -= num784;
			        num779 += num784;
			        num780 -= num784;
			        num781 += num784;

                    int num812 = num780;
                    int num813 = num781;
                    int num814 = num778;
                    int num815 = num779;

                    int num786 = WIDTH;

                    const int BEACH_WIDTH = 500;

                    bool isScanActive = false;

                    int num787 = WIDTH;

                    while (!isScanActive) {
                        isScanActive = true;

                        int midWorldX = _worldWidth / 2;
                        int fluffSpawnPointX = 250;

                        int biomeSizeX = biome.Width;

                        int x = _random.Next(midWorldX - num787, midWorldX + num787);
                        x = Math.Clamp(x, BEACH_WIDTH, _worldWidth - BEACH_WIDTH);
                        int startX = x - biomeSizeX,
                            endX = x + biomeSizeX;
                        int centerX = endX + (startX - endX) / 2;

                        if (startX < GenVars.evilBiomeBeachAvoidance) {
                            startX = GenVars.evilBiomeBeachAvoidance;
                        }

					    if (endX > _worldWidth - GenVars.evilBiomeBeachAvoidance) {
                            endX = _worldWidth - GenVars.evilBiomeBeachAvoidance;
                        }

					    if (x < startX + GenVars.evilBiomeAvoidanceMidFixer) {
                            x = startX + GenVars.evilBiomeAvoidanceMidFixer;
                        }

					    if (x > endX - GenVars.evilBiomeAvoidanceMidFixer) {
                            x = endX - GenVars.evilBiomeAvoidanceMidFixer;
                        }

					    if (startX < GenVars.dungeonLocation + num786 && endX > GenVars.dungeonLocation - num786) {
                            isScanActive = false;
                        }

                        if (startX < GenVars.dungeonLocation + num786 && endX > GenVars.dungeonLocation - num786) {
                            isScanActive = false;
                        }

                        foreach (BiomeData nextBiome in _biomes) {
                            if (nextBiome.ID == biome.ID)  {
                                continue;
                            }

                            if (x > nextBiome.X - nextBiome.Width && x < nextBiome.X + nextBiome.Width) {
                                isScanActive = false;
                            }
                        }

                        if (centerX > midWorldX - fluffSpawnPointX && centerX < midWorldX + fluffSpawnPointX) {
                            isScanActive = false;
                        }

						if (startX > midWorldX - fluffSpawnPointX && startX < midWorldX + fluffSpawnPointX) {
                            isScanActive = false;
                        }

						if (endX > midWorldX - fluffSpawnPointX && endX < midWorldX + fluffSpawnPointX) {
                            isScanActive = false;
                        }

						if (x > GenVars.UndergroundDesertLocation.X && x < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width) {
                            isScanActive = false;
                        }

						if (startX > GenVars.UndergroundDesertLocation.X && startX < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width) {
							isScanActive = false;
                        }

						if (endX > GenVars.UndergroundDesertLocation.X && endX < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width) {
							isScanActive = false;
                        }

						if (startX < num813 && endX > num812) {
							num812++;
							num813--;
							isScanActive = false;
						}

						if (startX < num815 && endX > num814) {
							num814++;
							num815--;
							isScanActive = false;
						}

                        if (isScanActive) {
                            _biomes[biome.ID].X = x;
                        }
                        else if (num787 < _worldWidth / 2 + _worldWidth / 3) {
                            num787 += _worldWidth / 3;
                        }
                    }
                }
            }

            void SetYForEachBiome() {
                foreach (BiomeData biome in _biomes) {
                    int id = biome.ID;

                    _biomes[id].Y = 250;
                    while (!TheTormentUtilities.SafeGetTile(biome.X, _biomes[id].Y).HasTile) {
                        _biomes[id].Y++;
                    }
                    _biomes[id].Y -= 1;
                }
            }

            SetXForEachBiome();
            SetYForEachBiome();
        }

        private void Step2_MakeShape() {
            AnnounceProgress("MakingShape", 0.2f);

            foreach (BiomeData biome in _biomes) {
                int x = biome.X, 
                    y = biome.Y;

                double width = biome.Width,
                       height = biome.Height;

                y += (int)height;

                int cleanUpAreaHeight = 50;

                height += cleanUpAreaHeight * 2;

                y += cleanUpAreaHeight / 2;

                Vector2D position = new(x, y);
                Vector2D velocity = new(_random.Next(-20, 21) * 0.2f, -5f);
                while (width > 0.0 && height > 0.0) {
                    //SetProgress(1.0 - height / 200.0);
                    width -= _random.Next(2);
                    height -= 1.0;

                    int num = (int)(position.X - width * 0.5);
                    if (num < 0) {
                        num = 0;
                    }

                    int num2 = (int)(position.X + width * 0.5);
                    if (num2 > _worldWidth) {
                        num2 = _worldWidth;
                    }

                    int num3 = (int)(position.Y - width * 0.5);
                    if (num3 < 0) {
                        num3 = 0;
                    }

                    int num4 = (int)(position.Y + width * 0.5);
                    if (num4 > _worldHeight) {
                        num4 = _worldHeight;
                    }

                    double num5 = width;
                    double num6 = position.Y + 2.0;
                    for (int k = num; k < num2; k++) {
                        if (height < cleanUpAreaHeight + 5) {
                            int num7 = num2 - num;
                            double num8 = num7 / 2.5;
                            bool flag = k > num + num7 / 5 && k < num + num8,
                                 flag2 = k > num + (num7 - num8),
                                 flag3 = k > num + num8 && !flag2;
                            if (flag && _random.NextBool(10)) {
                                num6 += -1;
                            }
                            if (flag2 && _random.NextBool(10)) {
                                num6 += 1;
                            }
                            if (flag3 && _random.NextBool(4)) {
                                num6 += _random.Next(-1, 2);
                            }
                            double num9 = 10.0;
                            num6 = Math.Clamp(num6, position.Y - num9, position.Y + num9);
                        }

                        for (int l = num3; l < num4; l++) {
                            if (l <= num6) {
                                continue;
                            }

                            double num7 = Math.Abs(k - position.X);
                            double num8 = Math.Abs(l - position.Y) * 3.0;
                            if (Math.Sqrt(num7 * num7 + num8 * num8) < num5 * 0.4)  {
                                ushort tileType = _tiles[k, l].TileType;
                                bool centerX = k == num + (num2 - num) / 2;
                                if (height > cleanUpAreaHeight) {
                                    biome.TilesToReplace.Add((tileType, new Point(k, l)));

                                    if (height == cleanUpAreaHeight + 1) {
                                        _biomes[biome.ID] = _biomes[biome.ID] with {
                                            X = (int)position.X - (int)width / 2,
                                            Y = (int)position.Y,
                                            Width = (int)width
                                        };
                                    }

                                    if (centerX) {
                                        _centerTiles.Add(new Point((int)position.X, (int)position.Y));
                                    }
                                }
                                else {
                                    biome.TilesToDestroy.Add((tileType, new Point(k, l)));

                                    int offset = 50;
                                    for (int k2 = -offset; k2 < offset; k2++) {
                                        int x2 = k + k2;
                                        for (int l2 = -offset; l2 < offset / 2; l2++) {
                                            int y2 = l + l2;
                                            Tile tile = _tiles[x2, y2];
                                            if (tile.HasTile && tile.TileType == TileID.Grass) {
                                                biome.TilesToReplace.Add((TileID.Grass, new Point(x2, y2)));
                                            }
                                        }
                                    }

                                    if (height == cleanUpAreaHeight - 1 && centerX) {
                                        _tunnelStartPosition = new Point((int)position.X, (int)position.Y);
                                    }
                                }
                            }
                        }
                    }

                    position += velocity;
                    velocity.X += _random.Next(-20, 21) * 0.01f;
                    if (velocity.X > 1f) {
                        velocity.X = 1f;
                    }
                    if (velocity.X < -1f) {
                        velocity.X = -1f;
                    }
                    if (velocity.Y > 1f) {
                        velocity.Y = -1f;
                    }
                    if (velocity.Y < -1f) {
                        velocity.Y = -1f;
                    }
                }
            }
        }

        private void Step3_BasePlacement() {
            AnnounceProgress("PlacingBase", 0.3f);

            ushort GetTileType(ushort tileType) {
                tileType = tileType switch {
                    TileID.Stone => _tileID_Stone,
                    TileID.Grass => _tileID_Grass1,
                    TileID.IceBlock => TileID.IceBlock,
                    TileID.SnowBlock => TileID.SnowBlock,
                    TileID.Sand => _tileID_Sand,
                    TileID.Mud => TileID.Mud,
                    _ => TileID.Dirt,
                };
                return tileType;
            }

            foreach (BiomeData biome in _biomes) {
                foreach ((ushort, Point) destroyTilePosition in biome.TilesToDestroy) {
                    Point position = destroyTilePosition.Item2;
                    if (InvalidTilesToDestroy.Contains(destroyTilePosition.Item1)) {
                        continue;
                    }
                    WorldGen.KillTile(position.X, position.Y);
                }
                foreach ((ushort, Point) placeTileInfo in biome.TilesToReplace) {
                    Point position = placeTileInfo.Item2;
                    ushort tileType = GetTileType(placeTileInfo.Item1);
                    if (InvalidTiles.Contains(tileType)) {
                        continue;
                    }
                    if (InvalidWalls.Contains(TheTormentUtilities.SafeGetTile(position.X, position.Y).WallType)) {
                        continue;
                    }
                    if (InvalidTilesToDestroy.Contains(tileType)) {
                        continue;
                    }
                    if ((tileType == TileID.Dirt || tileType == TileID.Mud) && position.Y > (int)(TheTormentUtilities.RockLayerMid - 30.0 * (double)TheTormentUtilities.WorldSize)) {
                        continue;
                    }
                    if (tileType == TileID.Dirt && position.Y < GenVars.worldSurfaceHigh && _random.NextBool(50)) {
                        tileType = _tileID_Grass1;
                    }
                    if (tileType == TileID.JungleGrass) {
                        tileType = _tileID_Grass2;
                    }
                    TheTormentUtilities.ReplaceTile(position.X, position.Y, tileType);
                }
            }
        }

        private void Step4_SpreadGrass() {
            AnnounceProgress("SpreadingGrass", 0.4f);

            double randomnessY = Main.worldSurface + 40.0;
            foreach (BiomeData biome in _biomes) {
                int max = biome.Width / 2 + biome.Width / 3;
                for (int index = 0; index < 3; index++) { 
                    for (int x = biome.Center.X - max; x < biome.Center.X + max; x++) {
				        randomnessY += (double)_random.Next(-2, 3);
                        randomnessY = Math.Clamp(randomnessY, Main.worldSurface + 30.0, Main.worldSurface + 50.0);

				        bool spreadWhen = false;
				        for (int y = (int)GenVars.worldSurfaceLow; (double)y < randomnessY; y++) {
                            Tile tile = _tiles[x, y];
                            if (tile.HasTile) {
						        if ((double)y < Main.worldSurface - 1.0 && !spreadWhen) {
							        if (tile.TileType == TileID.Dirt) {
								        WorldGen.grassSpread = TileID.Dirt;
                                        WorldGen.SpreadGrass(x, y, TileID.Dirt, _tileID_Grass1);
							        }
							        else if (tile.TileType == TileID.Mud) {
                                        WorldGen.grassSpread = 0;
                                        WorldGen.SpreadGrass(x, y, TileID.Mud, _tileID_Grass2);
							        }
						        }
						        spreadWhen = true;

                                if (tile.TileType == TileID.Grass) {
                                    tile.TileType = _tileID_Grass1;
                                }
                                else if (tile.TileType == TileID.JungleGrass) {
                                    tile.TileType = _tileID_Grass2;
                                }
                            }
				        }
			        }
                }
            }
        }

        private void Step5_CaveEntrance() {
            foreach (BiomeData biome in _biomes) {
                int x = biome.Center.X, y = 200;
                while (!TheTormentUtilities.SafeGetTile(x, y).HasTile) {
                    y++;
                }
                int num2 = 33 + _random.Next(-5, 6);
                int num3 = 25 + _random.Next(-3, 4);
                double num4 = 0.3;
                num4 *= 1.0 - _random.NextDouble() * 0.1;
                int num5 = _random.Next(95, 135);
                int num6 = (int)((double)num5 * num4);
                int num7 = num5 - num3;
                int num1 = num2 / 5;
                int num13 = num3 / 4;
                int num8 = num13 / 2;
                y += num8 - 5;
                int num9 = x - num5;
                int num10 = x + num5;
                int num11 = y - num5 - num7;
                int num12 = y + num5 + num7;
                int num18 = 0;
                int num19 = _random.Next(3, 8);
                for (int k = num11; k <= num12; k++) {
                    for (int l = num9; l <= num10; l++) {
                        int num15 = (int)Math.Sqrt(Math.Pow((double)Math.Abs(l - x) * (1.0 + _random.NextDouble() * 0.05), 2.0) + Math.Pow((double)(Math.Abs(k - y)) * (1.5 + _random.NextDouble() * 0.05), 2.0));
                        if (num15 < (int)((double)num6 * (1.0 + _random.NextDouble() * 0.025))) {
                            bool flag = num15 > (int)((double)num6 * (0.45 + _random.NextDouble() * 0.0625 + _random.NextDouble() * 0.0625));
                            int num16 = y + num13;
                            int num17 = k - (flag ? num8 : 0) + 3;
                            if (flag) { 
                                if (k < num16 + 8) {
                                    TheTormentUtilities.ReplaceTile(l, num17, _tileID_Stone);
                                    if (k > num16 - num1 + _random.Next(2) + num18 && k < num16) {
                                        WorldGen.KillTile(l, num17 - 2);
                                    }
                                }
                            }
                            else if (num15 < (int)((double)num6 * (0.55 + _random.NextDouble() * 0.01))) {
                                TheTormentUtilities.ReplaceTile(l, num17 + num19, _tileID_Stone);
                            }
                            _tiles[l, num17].WallType = 0;
                        }
                    }
                    if (_random.NextBool(50)) {
                        num18 -= k > (int)MathHelper.Lerp(num11, num12, 0.5f) ? -1 : 1;
                        num18 = (int)MathHelper.Clamp(num18, -2, 2);
                    }
                }
            }
        }

        private void Step6_FirstTunnel() {
            Point start = _tunnelStartPosition;
            while (_tiles[start.X, start.Y].HasTile) {
                start.Y--;
            }
            start.Y -= 2;
            var genRand = _random;
            int num = 50;
            Vector2D position = new(start.X, start.Y);
            Vector2D vector2D = new(0.0, 50.0 * 0.01);
            double num2 = 8;
            while (num > 0) {
                num--;

                for (int l = (int)(position.X - num2 / 2.0); (double)l < position.X + num2 / 2.0; l++) {
			        for (int m = (int)(position.Y - num2 / 2.0); (double)m < position.Y + num2 / 2.0; m++) {
				        //if (m > num) {
					       // if (Math.Abs((double)l - position.X) + Math.Abs((double)m - position.Y) < num2 * 0.3) {
						      //  Main.tile[l, m].active(active: false);
						      //  Main.tile[l, m].wall = 83;
					       // }
					       // else if (Math.Abs((double)l - position.X) + Math.Abs((double)m - position.Y) < num2 * 0.8 && Main.tile[l, m].wall != 83) {
						      //  Main.tile[l, m].active(active: true);
						      //  Main.tile[l, m].type = 203;
						      //  if (Math.Abs((double)l - position.X) + Math.Abs((double)m - position.Y) < num2 * 0.6)
							     //   Main.tile[l, m].wall = 83;
					       // }
				        //}
				        /*else*/ if (Math.Abs((double)l - position.X) + Math.Abs((double)m - position.Y) < num2 * 0.3 && Main.tile[l, m].HasTile) {
                            //TheTormentUtilities.ReplaceTile(l, m, TileID.GoldBrick);
                            WorldGen.KillTile(l, m);
				        }
			        }
		        }

                if (num < 45) {
                    if (num < 25) {
                        vector2D.X += (double)genRand.Next(-50, 51) * 0.005;
                        num2 += (double)genRand.Next(-50, 51) * 0.025;

                        if (num2 < 10.0)
                            num2 = 10.0;
                    }
                    else {
                        num2 += (double)genRand.Next(40, 51) * 0.015;
                    }

                    if (num2 > 18.0)
                        num2 = 18.0;
                }

                vector2D.Y += (double)genRand.Next(-50, 51) * 0.01;
                if (vector2D.Y < 0.25)
                    vector2D.Y = 0.25;

                if (vector2D.Y > 2.0)
                    vector2D.Y = 2.0;

                if (vector2D.X < -1.0)
                    vector2D.X = -1.0;

                if (vector2D.X > 1.0)
                    vector2D.X = 1.0;

                position += vector2D;
            }
        }

        private void AnnounceProgress(string step,
                                      float progress = -1f) {
            if (_progress == null) {
                return;
            }

            string key = "WorldGen.TheTormentBiome.";
            _progress.Message = TheTormentUtilities.ModText(key + "Generating").Value + TheTormentUtilities.ModText(key + step).Value;

            if (progress == -1) {
                return;
            }
            SetProgress(progress);
        }

        private void SetProgress(double progress)
            => _progress?.Set(progress);
    }
}
