using System;

using Terraria.ModLoader;

namespace TheTorment;

public sealed class TheTorment : Mod {
    public const string PATHTO_GRAPHICSFOLDER = $"{nameof(TheTorment)}/Source/Graphics";

    internal static TheTorment? _instance;

    public static TheTorment Instance
        => _instance ?? throw new InvalidOperationException("An instance of the mod has not yet been created.");

    internal TheTorment() {
        _instance = this;
    }
}