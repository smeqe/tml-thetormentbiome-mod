using System.Collections.Generic;

using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheTorment.Content.Gameplay.WorldGeneration.TheTorment;

public sealed partial class TheTormentBiome : ModSystem {
    private const string GEN_LAYER_NAME = "The Torment";

    public override void ModifyWorldGenTasks(List<GenPass> tasks,
                                             ref double totalWeight) {
        int corriptionGenIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Corruption"));
        tasks.RemoveAt(corriptionGenIndex);

        int dungeonGenIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
        if (dungeonGenIndex == -1) {
            return;
        }
        tasks.Insert(dungeonGenIndex + 1, new TheTormentBiomePass(GEN_LAYER_NAME, 0f));
    }
}
