using System.Collections.Generic;

using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheTorment.Content.Gameplay.World.Generation.TheTorment;

public sealed partial class TheTormentBiome : ModSystem {
    private const string GEN_LAYER_NAME = "The Torment";
    private const float LAYER_WEIGHT = 600f;

    public override void ModifyWorldGenTasks(List<GenPass> tasks,
                                             ref double totalWeight) {
        int corriptionGenIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Corruption"));
        tasks.RemoveAt(corriptionGenIndex);

        int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
        if (genIndex != -1) {
            tasks.Insert(genIndex + 1, new TheTormentBiomePass(GEN_LAYER_NAME, LAYER_WEIGHT));
        }

        genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
        if (genIndex == -1) {
            return;
        }
        tasks.Insert(genIndex + 1, new TheTormentBiomePassCleanUp(GEN_LAYER_NAME, LAYER_WEIGHT));
    }
}
