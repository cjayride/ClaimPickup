# Claim Pickup

## 1.0.2

- Area-harvest drops on the ground are claimed during auto-pickup, so they are not left waiting on the server.

When auto loot or an area harvest picks up a ground drop, a plant, or a beehive, your client takes ZDO ownership first. The pickup then runs locally instead of waiting on whoever loaded that zone first.

Ship and cart objects are not claimed. If another player is within `NearbyMeters` (default 5), the claim is skipped so you do not both take the same drop.

**Client only.** Dedicated servers do not need this. Both players need it.

## Install

Copy `Cjayride.ClaimPickup.dll` into `BepInEx/plugins`.
