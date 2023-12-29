using HarmonyLib;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Patches
{
    public class EventPatches
    {

        [HarmonyPatch]
        public static class Patches
        {
            /*[HarmonyPatch(typeof(PlayerClothing))]
            [HarmonyPatch("ReceiveVisualToggleState")]
            [HarmonyPrefix]
            static void ReceiveVisualToggleState(ref EVisualToggleType type, ref bool toggle)
            {
                toggle = true;
            }*/
            // prefix patching ReceiveVisualToggleRequest instead of ReceiveVisualToggleState because patch above was letting through one request and then working.
            [HarmonyPatch(typeof(PlayerClothing))]
            [HarmonyPatch("ReceiveVisualToggleRequest")]
            [HarmonyPrefix]
            static void ReceiveVisualToggleRequest(ref EVisualToggleType type)
            {
                type = EVisualToggleType.SKIN;
            }
        }

    }
}