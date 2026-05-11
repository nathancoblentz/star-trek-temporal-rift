using System.Collections.Generic;
using System.Drawing;
/*
 * Module: Enemy.cs
 * Purpose: Base class for all enemy types.
 * Functionality:
 *  - Defines common enemy stats (HP, Strength, etc.).
 *  - Provides virtual methods for turn logic (OnTurnStart) and combat actions.
 *  - Implements basic damage dealing and receiving logic.
 */
using FinalProject.Interfaces;

namespace FinalProject.Classes
{
    public class Enemy : Character
    {
        public Image EnemyImage { get; set; }
 
         public Enemy() : base("Unknown", 10, 1, 1, 1, 1, new List<IAbility>()) { }
 
         public Enemy(string name, int hp, int str, int dex, int def, int wis, List<IAbility> abilities, Image img = null)
             : base(name, hp, str, dex, def, wis, abilities)
         {
             EnemyImage = img;
         }

        // Base enemy defense can be overridden
        public virtual int GetDefense() => Defense;

        public virtual void OnTurnStart(FinalProject.GameLogic.CombatEngine engine) 
        {
            UpdateStatus();
        }

    }
}
