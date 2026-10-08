using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Cjayride.ClaimPickup
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "cjayride.ClaimPickup";
        public const string PluginName = "Claim Pickup";
        public const string PluginVersion = "1.0.2";

        public static ConfigEntry<float> NearbyMeters;

        private void Awake()
        {
            NearbyMeters = Config.Bind("General", "NearbyMeters", 5f,
                new ConfigDescription("Skip the claim when another player is this close, so stacked combat does not duplicate a drop. Solo honey and plants still claim.", new AcceptableValueRange<float>(1f, 16f)));
            new Harmony(PluginGuid).PatchAll();
            Logger.LogInfo("Ground loot, plants, and beehives claim ownership on your client before pickup, unless another player is already close.");
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
            if (OtherPlayerNearby(thing.transform.position))
                return;

            view.ClaimOwnership();
        }

        static bool OtherPlayerNearby(Vector3 pos)
        {
            var players = Player.GetAllPlayers();
            if (players == null)
                return false;

            float range = Plugin.NearbyMeters != null ? Plugin.NearbyMeters.Value : 5f;
            float limit = range * range;
            for (int i = 0; i < players.Count; i++)
            {
                Player other = players[i];
                if (other == null || other == Player.m_localPlayer)
                    continue;
                if ((other.transform.position - pos).sqrMagnitude <= limit)
                    return true;
            }

            return false;
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

    // Autopick never calls Pickup until this client already owns the drop. It only
    // calls RequestOwn, which waits on the server. Claim here so the same check
    // can pick the stack up.
    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.CanPickup))]
    internal static class ClaimBeforeAutoPickup
    {
        private static void Prefix(ItemDrop __instance)
        {
            if (__instance == null || Player.m_localPlayer == null)
                return;
            Claim.Take(__instance, __instance.m_nview, Player.m_localPlayer);
        }
    }

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.RequestOwn))]
    internal static class ClaimInsteadOfRequest
    {
        private static bool Prefix(ItemDrop __instance)
        {
            if (__instance == null || Player.m_localPlayer == null)
                return true;
            if (__instance.m_nview != null && __instance.m_nview.IsOwner())
                return true;
            Claim.Take(__instance, __instance.m_nview, Player.m_localPlayer);
            return __instance.m_nview == null || !__instance.m_nview.IsOwner();
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
