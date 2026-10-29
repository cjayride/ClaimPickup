# ClaimPickup — Valheim 1.0.21 release

## Goal
- Verify Release build against Valheim 1.0.21, update BepInExPack to 5.4.2360, changelog, Thunderstore `/dist` zip, git tag `v1.0.3`.

## Status
- Completed

## Changes
- Version 1.0.3 in csproj, Plugin.cs, manifest.
- BepInEx: `denikson-BepInExPack_Valheim-5.4.2360`.
- CHANGELOG (root + Package).
- `dist/` Thunderstore artifacts and zip.

## Commands
- `dotnet build -c Release` — succeeded (0 warnings, 0 errors).

## Output
- DLL: `D:\AI-Agent-Projects\cjaycraft\Cjayride.ClaimPickup\bin\Release\net481\Cjayride.ClaimPickup.dll` (FileVersion 1.0.3.0)
- Zip: `D:\AI-Agent-Projects\cjaycraft\Cjayride.ClaimPickup\dist\cjayride-ClaimPickup-1.0.3.zip`
