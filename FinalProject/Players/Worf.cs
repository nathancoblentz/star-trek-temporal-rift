using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;
using System;
using System;

namespace FinalProject.Players
{
    public class Worf : Player
    {
        public Worf() : base(
            "Worf",
            150, 18, 12, 16, 10,
            new List<IAbility>
            {
                new BatlethSlash(),
                new WarriorsRoar()
            })
        { }
    }

    public class BatlethSlash : IAbility
    {
        public string Name => "Bat'leth Slash";
        public string Description => "Powerful melee attack with Worf's Bat'leth.";
        public int Cooldown => 3;
        public int CooldownRemaining { get; set; } = 0;

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            int dmg = p.Strength + 10;
            int taken = target.TakeDamage(dmg);
            // Cooldown handled by CombatEngine
            return $"{p.Name} roars, 'Today is a good day to die!' and swings his Bat'leth! ⚔️\r\n" +
                $"Deals {taken} damage to {((Character)target).Name}!";
        }
    }

    public class WarriorsRoar : IAbility
    {
        public string Name => "Warrior's Roar";
        public string Description => "Intimidates the enemy, reducing their defense.";
        public int Cooldown => 3;
        public int CooldownRemaining { get; set; } = 0;

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            if (target is Character enemy)
            {
                if (enemy is Q)
                {
                    enemy.TakeDamage(9999);
                    return $"{p.Name} lets out a primal scream that shakes the cosmos! 🦁\r\n" +
                        $"Q covers his ears. 'How barbaric! I shan't stay for this noise.'\r\n" +
                        $"Q vanishes! (Victory by Intimidation)";
                }

                // Apply temporary debuff
                enemy.TempDefenseMod = -5;
                enemy.StatusDuration = 2; // Lasts for next enemy turn + player turn

                return $"{p.Name} lets out a primal scream! The enemy trembles! 🦁\r\n" +
                    $"{enemy.Name}'s defense reduced by 5 for 1 round!";
            }
            return null;
        }
    }
}
