# 1.0.2

- Ground drops from area harvest are claimed as soon as auto-pickup looks at them, instead of waiting on a RequestOwn to the server. Still skipped when another player is within NearbyMeters.

# 1.0.1

- Do not claim a drop if another player is within NearbyMeters (default 5). Stacked combat stays on vanilla ownership so both of you do not pick up the same shield.

# 1.0.0

- Claim ownership on your client before a ground drop, plant, or beehive is picked up, so auto loot and area harvest do not wait on whoever loaded the zone first.
- Ships and carts are left alone.
- Client-side. Dedicated server does not need it.
