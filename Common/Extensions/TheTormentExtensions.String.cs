namespace TheTorment.Common.Extensions;

public static partial class TheTormentExtensions {
    public static string TheTormentTexturePath(this string texturePath) {
        return texturePath.Replace("Gameplay", "Graphics/Textures");
    }
}