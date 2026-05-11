using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;

namespace FinalProject.Players
{
    public class Spock : Player
    {
        public Spock() : base(
            "Spock",
            130, 14, 14, 12, 20,
            new List<IAbility>
            {
                new VulcanNervePinch(),
                new LogicalCounterattack()
            })
        { }
    }

    public class VulcanNervePinch : IAbility
    {
        public string Name => "Vulcan Nerve Pinch";
        public string Description => "Instantly incapacitates enemy for one turn.";
        public int Cooldown => 3;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            // Example: skip enemy turn (handled in CombatEngine)
            int dmg = p.Strength + 5;
            int taken = target.TakeDamage(dmg);
            
            // Apply Stun
            ((Character)target).StunnedTurns = 1;

            return $"{p.Name} calmly reaches out to the enemy's shoulder... 'Sleep.' 🖖\r\n" +
                $"{((Character)target).Name} collapses momentarily! (Dealt {taken} damage, Stunned)";
        }
    }

    public class LogicalCounterattack : IAbility
    {
        public string Name => "Logical Counterattack";
        public string Description => "Reflects part of incoming damage.";
        public int Cooldown => 2;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            
            if (target is Q)
            {
                target.TakeDamage(9999);
                return $"{p.Name} raises an eyebrow. 'Your existence is illogical.'\r\n" +
                    $"Q frowns. 'You Vulcans are no fun at all. I'm leaving.'\r\n" +
                    $"Q vanishes in a puff of logic! (Victory)";
            }

            // Use enemy's strength against them
            int dmg = p.Strength + (((Character)target).Strength / 2);
            int taken = target.TakeDamage(dmg);
            return $"{p.Name} calculates the optimal strike vector, using the enemy's momentum.\r\n" +
                $"Logic dictates this result. (Dealt {taken} damage)";
        }
    }
}
