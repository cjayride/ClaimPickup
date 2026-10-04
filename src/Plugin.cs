using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Cjayride.ClaimPickup
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "cjayride.ClaimPickup";
        public const string PluginName = "Claim Pickup";
        public const string PluginVersion = "1.0.0";

        private void Awake()
        {
            new Harmony(PluginGuid).PatchAll();
            Logger.LogInfo("Ground loot, plants, and beehives claim ownership on your client before pickup.");
        }
    }

    internal static class Claim
    {
        public static void Take(Component thing, ZNetView view, Humanoid character)
        {
            if (thing == null || character == null || view == null || !view.IsValid())
                return;
            if (Player.m_localPlayer == null || character != Player.m_localPlayer)
                return;
            if (view.IsOwner())
                return;
            if (thing.GetComponentInParent<Ship>() != null)
                return;
            if (thing.GetComponentInParent<Vagon>() != null)
                return;

            view.ClaimOwnership();
        }
    }

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Pickup))]
    internal static class ClaimItemDrop
    {
        private static void Prefix(ItemDrop __instance, Humanoid character)
        {
            if (__instance == null)
                return;
            Claim.Take(__instance, __instance.m_nview, character);
        }
    }

    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
    internal static class ClaimPickable
    {
        private static void Prefix(Pickable __instance, Humanoid character)
        {
            if (__instance == null)
                return;
            Claim.Take(__instance, __instance.m_nview, character);
        }
    }

    [HarmonyPatch(typeof(Beehive), nameof(Beehive.Interact))]
    internal static class ClaimBeehive
    {
        private static void Prefix(Beehive __instance, Humanoid character)
        {
            if (__instance == null)
                return;
            Claim.Take(__instance, __instance.m_nview, character);
        }
    }
}
