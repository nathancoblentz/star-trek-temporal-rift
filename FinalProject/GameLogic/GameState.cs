/*
 * Module: GameState.cs
 * Purpose: Manages the persistent state of the game.
 * Functionality:
 *  - Stores player inventory, actions taken, and visited flags.
 *  - Handles serialization (Save) and deserialization (Load) of game data to JSON.
 *  - Ensures game progress is preserved between sessions.
 */
using System;
using System.Collections.Generic;
using System.Text.Json;
using FinalProject.Classes;

namespace FinalProject.GameLogic
{
    public class GameState
    {
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }

        // Track inventory and actions
        public List<string> Inventory { get; set; } = new();
        public List<string> ActionsTaken { get; set; } = new();

        // Remember selected player
        public Player CurrentPlayer { get; set; }

        // Use this to show start message ONLY when the is starting.
        public bool HasSeenStartMessage { get; set; } = false;

        public int StoredQHealth { get; set; } = -1;

        // Save the game state to a JSON file
        public void Save(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }

        // Load the game state from a JSON file
        public static GameState Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new GameState();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameState>(json) ?? new GameState();
        }
    }

}
