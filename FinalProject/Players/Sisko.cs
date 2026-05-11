using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;

namespace FinalProject.Players
{
    public class Sisko : Player
    {
        public Sisko() : base(
            "Benjamin Sisko",
            140, 16, 12, 15, 14,
            new List<IAbility>
            {
                new SuckerPunch(),
                new CommandersGambit()
            })
        { }
    }

    public class SuckerPunch : IAbility
    {
        public string Name => "Sucker Punch";
        public string Description => "Deals high-damage, risky attack.";
        public int Cooldown => 3;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            
            if (target is Q)
            {
                target.TakeDamage(9999);
                return $"{p.Name} punches Q right in the face! 👊\r\n" +
                    $"Q stumbles back, holding his nose. 'You hit me! Picard never hit me!'\r\n" +
                    $"He sneers. 'I'm not staying where I'm not wanted. Goodbye!'\r\n" +
                    $"Q vanishes in a flash of light! (Insta-Defeat)";
            }

            int dmg = p.Strength + 10;
            int taken = target.TakeDamage(dmg);
            return $"{p.Name} winds up and delivers a massive hook! 👊\r\n" +
                $"'You're not Q, but that felt good!' (Dealt {taken} damage)";
        }
    }

    public class CommandersGambit : IAbility
    {
        public string Name => "Commander's Gambit";
        public string Description => "High-risk high-reward strike.";
        public int Cooldown => 4;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            int dmg = p.Strength * 2;
            int taken = target.TakeDamage(dmg);
            
            // Self-damage (ignoring defense)
            p.CurrentHP -= 5;
            if (p.CurrentHP < 0) p.CurrentHP = 0;

            return $"{p.Name} executes a risky maneuver! The Defiant would be proud! 🚀\r\n" +
                $"Deals {taken} damage, but takes 5 recoil damage!";
        }
    }
}
