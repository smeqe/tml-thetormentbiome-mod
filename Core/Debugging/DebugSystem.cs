using System;

using Terraria;
using Terraria.ModLoader;

namespace TheTorment.Core.Debugging;

internal sealed class DebugSystem : ModSystem {
    internal static void Log(object text, bool toChat = false, bool toConsole = false, bool toFile = false) {
        string? textToDisplay = text?.ToString();

		if (toChat) {
			Main.NewText(textToDisplay);
		}

        if (toConsole && !Main.dedServ) {
            Console.WriteLine(textToDisplay);
        }

        if (toFile) {
            TheTorment.Instance.Logger.Debug(textToDisplay);
        }
	}
}