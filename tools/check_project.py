#!/usr/bin/env python3
"""Structural check for the Unity project. Runs without Unity installed.

It cannot compile C#, so it verifies what it can: required files, the manifest,
and that the roster matches nrcu-kart.roster/v1.
"""
from __future__ import annotations

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).parents[1]

REQUIRED = [
    "Packages/manifest.json",
    "ProjectSettings/ProjectVersion.txt",
    "Assets/Data/Roster/roster.json",
    "Assets/Scripts/Kart/KartController.cs",
    "Assets/Scripts/Track/Checkpoint.cs",
    "Assets/Scripts/Track/Respawn.cs",
    "Assets/Scripts/Race/RaceManager.cs",
    "Assets/Scripts/Race/PositionTracker.cs",
    "Assets/Scripts/Items/ItemBox.cs",
    "Assets/Scripts/Roster/RosterEntry.cs",
    "README.md",
]

PLACEHOLDER = re.compile(r"TODO|FIXME|NotImplementedException", re.I)
NS = re.compile(r"namespace NRCU\.")


def main() -> int:
    problems: list[str] = []

    for rel in REQUIRED:
        if not (ROOT / rel).exists():
            problems.append(f"missing {rel}")

    manifest = ROOT / "Packages" / "manifest.json"
    if manifest.exists():
        data = json.loads(manifest.read_text())
        deps = data.get("dependencies", {})
        for needed in ("com.unity.inputsystem", "com.unity.render-pipelines.universal"):
            if needed not in deps:
                problems.append(f"manifest missing {needed}")

    roster_path = ROOT / "Assets" / "Data" / "Roster" / "roster.json"
    if roster_path.exists():
        roster = json.loads(roster_path.read_text())
        if roster.get("schema") != "nrcu-kart.roster/v1":
            problems.append("roster schema is not nrcu-kart.roster/v1")
        slugs = [c.get("slug") for c in roster.get("characters", [])]
        if len(set(slugs)) != len(slugs):
            problems.append("duplicate roster slugs")
        named = [json.dumps(c).lower() for c in roster.get("characters", [])]
        if not any("sovthpaw" in blob for blob in named):
            problems.append("roster has no sovthpaw entry")
        print(f"roster: {roster.get('count')} characters")

    for script in (ROOT / "Assets" / "Scripts").rglob("*.cs"):
        text = script.read_text()
        if PLACEHOLDER.search(text):
            problems.append(f"{script.relative_to(ROOT)}: placeholder left in source")
        if not NS.search(text):
            problems.append(f"{script.relative_to(ROOT)}: missing NRCU namespace")

    print(f"scripts: {len(list((ROOT / 'Assets' / 'Scripts').rglob('*.cs')))}")
    for problem in problems:
        print(f"  ! {problem}")
    print("ok" if not problems else f"{len(problems)} problem(s)")
    return 1 if problems else 0


if __name__ == "__main__":
    raise SystemExit(main())
