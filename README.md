# Claim Pickup

When auto loot or an area harvest picks up a ground drop, a plant, or a beehive, your client takes ZDO ownership first. The pickup then runs locally instead of waiting on whoever loaded that zone first.

Ship and cart objects are not claimed.

**Client only.** Dedicated servers do not need this. Both players need it.

## Install

Copy `Cjayride.ClaimPickup.dll` into `BepInEx/plugins`.
