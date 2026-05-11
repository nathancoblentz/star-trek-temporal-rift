/*
 * Module: Tribble.cs
 * Purpose: Implements the Tribble enemy type.
 * Functionality:
 *  - Features aggressive stat scaling based on generation.
 *  - Implements probabilistic breeding logic (multiplication).
 *  - Overrides OnTurnStart to handle growth and reproduction mechanics.
 */
using System.Collections.Generic;
using System.Drawing;
using FinalProject.Interfaces;

namespace FinalProject.Classes
{
    public class Tribble : Enemy
    {
        private int roundsAlive = 0;
        private bool threatened = false;
        private int generation;
        private static System.Random rng = new System.Random();

        // Cute defense wears off as they age (roundsAlive increases)
        public int CuteDefenseBonus => roundsAlive < 4 ? 100 : 0;

        public Tribble(Image img = null, int generation = 1)
            : base(
                  name: $"Tribble (Gen {generation})",
                  hp: 10 + (generation * 5), // Aggressive HP scaling
                  str: 2 + (generation * 3), // Aggressive Strength scaling
                  dex: 2,
                  def: 6,
                  wis: 1,
                  abilities: new List<IAbility>(),
                  img: img
            )
        {
            this.generation = generation;
        }

        public override void OnTurnStart(FinalProject.GameLogic.CombatEngine engine)
        {
            roundsAlive++;

            // Swarm Logic: Strength increases by the number of Tribbles present
            int swarmCount = engine.AliveEnemyCount;
            if (swarmCount > 1)
            {
                this.Strength += swarmCount;
                engine.Form.AppendLog($"{this.Name} gains +{swarmCount} Strength from the swarm! 🐀");
            }

            // 90% chance to breed if threatened
            if (threatened && rng.NextDouble() < 0.9)
            {
                // Breed!
                var newTribble = new Tribble(this.EnemyImage, this.generation + 1);
                engine.Form.AppendLog($"A Tribble multiplies! (Gen {newTribble.generation}) 🐾");
                engine.SpawnEnemy(newTribble);
            }
        }

        public override int TakeDamage(int dmg)
        {
            threatened = true;

            // Cute defense bonus
            int effectiveDmg = System.Math.Max(dmg - CuteDefenseBonus, 0);
            if (effectiveDmg <= 0)
            {
                return 0; // No damage taken
            }

            return base.TakeDamage(effectiveDmg);
        }

        public override string GetDefeatMessage()
        {
            return $"{Name} squeaks and pops! 💥";
        }

        public override string GetHitMessage(int damage)
        {
            if (damage == 0)
            {
                return "You hesitate... you can't attack something so cute! 💖 (0 damage)";
            }
            return null;
        }
    }
}
