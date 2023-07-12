using Terraria.Localization;

namespace TheTorment.Common.Utilities;

public static partial class TheTormentUtilities {
    public const string KEY = $"Mods.{nameof(TheTorment)}.";

    public static LocalizedText ModText(string key)
        => Language.GetText(KEY + key);
}