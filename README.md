# Exorcism Block-18

> A dark fantasy hardcore vampire survivor-like game built with Godot 4 and C#.

Fight through waves of enemies, collect procedurally generated loot, level up your character and survive as long as possible.

---

## Gameplay

- **Combat** — Real-time combat with attack aimed toward the cursor
- **Enemies** — Goblin, Goblin Archer, Ogre with unique AI behaviors
- **Loot** — Procedurally generated items with randomized stats and tiered rarity
- **Inventory** — 9 equipment slots (Weapon, Helmet, Armor, Gloves, Boots, 2x Ring, Amulet, Talisman)
- **Merchant** — Daily-refreshing shop with reroll system
- **Difficulty** — 5 difficulty levels: Hollow / Cursed / Abyssal / Eldritch / Forsaken
- **Progression** — XP system, level-up skills, meta-progression via BestScore

---

## Tech Stack

| | |
|---|---|
| Engine | Godot 4.6.1 |
| Language | C# (.NET 10.0) |
| Physics | Jolt Physics |
| Rendering | Forward Plus + D3D12 |
| Resolution | 1152x648 |

---

## Getting Started

### Requirements
- [Godot 4.6.1 .NET](https://godotengine.org/download/)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Run
1. Clone the repository
```bash
git clone https://github.com/bobrzlober/Exorcism-Block-18.git
```
2. Open `project.godot` in Godot 4
3. Press `F5` to run

---

## Project Structure

```
/data          — JSON templates (items, talismans)
/scripts       — C# game logic
/scences       — Godot scene files (.tscn)
/sprites       — Art assets
```

---

## Authors

- **Sk1y-1**
- **BragarArtem**

---

## License

[MIT](LICENSE)
