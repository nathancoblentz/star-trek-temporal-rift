# Star Trek: Temporal Rift

A turn-based text-adventure RPG set aboard a derelict starship. Choose your Starfleet officer, navigate a maze of rooms, fight enemies with a turn-based combat system, collect items, and retrieve the Time Crystal to save the timeline.

**[▶ Play in Browser](https://nathancoblentz.github.io/star-trek-temporal-rift/)**

Originally built as a **C# WinForms** desktop application, then ported to a **vanilla JavaScript** browser game — both versions are included in this repository.

---

## Project Overview

The game features four playable characters (Picard, Worf, Spock, Sisko), five enemy types with unique AI behaviors, an 8-room explorable map with locked doors and conditional progression, an inventory/action system, and persistent save/load functionality.

### Key Gameplay Systems

| System | Description |
|---|---|
| **Turn-Based Combat** | Player attacks, special abilities with cooldowns, enemy AI turns with status effects (stun, debuffs) |
| **Room Navigation** | Grid-based maze with conditional door locks — progression gates based on inventory and completed actions |
| **Inventory & Actions** | Items (Phaser, Keycard, Combadge) unlock new abilities and paths; actions track game progress |
| **Enemy AI** | Each enemy type has distinct behavior — Tribbles breed exponentially, Q teleports players to random rooms, Romulans cloak |
| **Save/Load** | C# version writes to JSON file; JS version uses `localStorage` |

---

## Architecture

### C# WinForms (Original)

```
FinalProject/
├── Classes/          # Character (abstract base), Player, Enemy, ThemeManager
├── Players/          # Picard, Worf, Spock, Sisko — each with 2 unique abilities
├── Enemies/          # Bat, RomulanScout, BorgDrone, Q, Tribble, EnemyFactory
├── Interfaces/       # IAbility, ICombatant (IAttackable + IAttackableTarget), ICombatContext
├── GameLogic/        # CombatEngine, Maze (room definitions + logic), GameState, Room
├── Acquired Abilities/ # CallScottyAbility (granted mid-game via item pickup)
├── Forms/            # StartForm, CharacterSelectForm, GameForm (main game UI)
└── Program.cs        # Entry point
```

### JavaScript Browser Port

```
star_trek_game_js/
├── css/styles.css    # LCARS-inspired dark theme (custom CSS)
├── index.html        # Single-page app with three screen states
└── js/
    ├── Character.js  # Base class with stats, damage calc, status effects
    ├── Player.js     # 4 player subclasses + factory/restore for save system
    ├── Enemy.js      # Base enemy class with onTurnStart hook
    ├── Enemies.js    # 5 enemy types + EnemyFactory
    ├── Abilities.js  # 9 ability classes with activate/cooldown pattern
    ├── Room.js       # Room data class
    ├── Maze.js       # 8-room map with enter/UI logic callbacks
    ├── GameState.js  # localStorage-backed persistence
    ├── CombatEngine.js # Async turn-based combat (await/delay pattern)
    ├── GameForm.js   # DOM-based game controller (replaces WinForms)
    └── app.js        # Screen management, character select, entry point
```

---

## Technical Skills Demonstrated

### Object-Oriented Design
- **Inheritance hierarchy**: `Character` → `Player` → `Picard/Worf/Spock/Sisko` and `Character` → `Enemy` → `Bat/Q/Tribble/etc.`
- **Interface segregation**: `ICombatant` composes `IAttackable` + `IAttackableTarget`; `IAbility` defines a contract for all special abilities
- **Strategy pattern**: Each ability is an independent class implementing `IAbility.Activate()` — abilities are composed onto characters, not hardcoded
- **Factory pattern**: `EnemyFactory` for encounter generation; `createPlayer()` / `restorePlayer()` in JS for save/load

### Asynchronous Programming
- C#: `async/await` with `Task.Delay()` for paced combat turns
- JS: `async/await` with `Promise`-based `delay()` — direct translation of the same pattern

### State Management
- Game state serialization/deserialization (C# `System.Text.Json`, JS `JSON.stringify`/`localStorage`)
- Conditional game progression tracked via `actionsTaken` and `inventory` arrays
- Room logic split into `RoomEnterLogic` (narrative) and `RoomUILogic` (available actions) — separation of concerns within each room

### Cross-Platform Migration
- Ported a .NET 9 WinForms desktop app to a zero-dependency browser application
- Mapped `System.Windows.Forms` controls → DOM manipulation
- Replaced file I/O (`File.WriteAllText`) → `localStorage` API
- Replaced `Task.Delay()` → `setTimeout`/`Promise` async pattern
- Replaced `MessageBox.Show()` → Bootstrap modal dialogs
- Preserved identical game logic, combat math, and room behavior across both implementations

### Front-End Development
- Responsive layout with Bootstrap 5 grid
- Custom CSS theme (LCARS-inspired) with CSS custom properties, `@keyframes` animations, and Google Fonts
- DOM event handling, dynamic element creation, screen state management without a framework

---

## How to Run

### Browser Version (Recommended)
Visit **[nathancoblentz.github.io/star-trek-temporal-rift](https://nathancoblentz.github.io/star-trek-temporal-rift/)** — no install required.

Or serve locally:
```bash
# Any static file server works
cd star_trek_game_js
npx serve .
```

### C# Desktop Version
Requires [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) on Windows:
```bash
cd FinalProject
dotnet run
```

---

## Game Map

```
                    [0,1]
              Auxiliary Control
                    |
 [-1,0]          [0,0]          [1,0]
Engineering ── Transporter ── Captain's
  Core            Room        Ready Room
                    |
[-1,-1]          [0,-1]
Sickbay ──── Communications
                Array
                    |
[-1,-2]          [0,-2]         [1,-2]
 Cargo    ────   Bridge   ──── Security
  Bay                           Office
```

---

## Technologies

| | C# Version | JS Version |
|---|---|---|
| **Language** | C# 12 / .NET 9 | JavaScript (ES6+) |
| **UI** | Windows Forms | HTML5 / CSS3 / DOM |
| **Styling** | ThemeManager (programmatic) | Custom CSS + Bootstrap 5 |
| **Persistence** | JSON file I/O | localStorage |
| **Async** | Task.Delay / async-await | Promise / async-await |
| **Deployment** | Desktop executable | GitHub Pages (static) |
