using System.Collections.Generic;
/*
 * Module: Player.cs
 * Purpose: Defines the player character and their attributes.
 * Functionality:
 *  - Stores player stats (HP, Strength, Defense, etc.).
 *  - Manages player inventory and special abilities.
 *  - Provides methods for taking damage and dealing damage.
 *  - Serves as the base for specific character classes (Spock, Sisko, etc.).
 */
using FinalProject.Interfaces;

namespace FinalProject.Classes
{
    public class Player : Character
    {
        public Player() { }

        public Player(string name, int hp, int str, int dex, int def, int wis, List<IAbility> abilities)
            : base(name, hp, str, dex, def, wis, abilities)
        {
        }
    }
}
