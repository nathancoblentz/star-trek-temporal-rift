using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;

namespace FinalProject.Players
{
    public class Picard : Player
    {
        public Picard() : base(
            "Jean-Luc Picard",
            120, 10, 12, 14, 18,
            new List<IAbility>
            {
                new InspiringSpeech(),
                new PrecisionStrike()
            })
        { }
    }

    public class InspiringSpeech : IAbility
    {
        public string Name => "Inspiring Speech";
        public string Description => "Bores the enemy into submission with a long speech.";
        public int Cooldown => 3;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            var t = (Character)target;
            
            if (t is Q)
            {
                t.TakeDamage(9999);
                return $"{p.Name} begins a long speech about humanity's potential...\r\n" +
                    $"Q rolls his eyes. 'Oh, spare me the lecture, Jean-Luc! I'm leaving!' 🙄\r\n" +
                    $"He snaps his fingers. 'You are far too boring today.'\r\n" +
                    $"Q vanishes! (Victory by Boredom)";
            }

            // Debuff enemy
            // Debuff enemy temporarily
            t.TempStrengthMod = -2;
            t.TempDefenseMod = -2;
            t.StatusDuration = 2;

            return $"{p.Name} begins a long, impassioned speech about the Federation's values...\r\n" +
                $"{t.Name} looks visibly bored and distracted! 😴 (-2 STR, -2 DEF for 1 round)";
        }
    }

    public class PrecisionStrike : IAbility
    {
        public string Name => "Precision Strike";
        public string Description => "Deals extra damage ignoring some defense.";
        public int Cooldown => 2;
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            var p = (Character)user;
            int dmg = p.Strength + 5;
            int taken = target.TakeDamage(dmg);
            return $"{p.Name} uses {Name}, dealing {taken} damage to {((Character)target).Name}!";
        }
    }
}
