/*
 * Module: CallScottyAbility.cs
 * Purpose: Implements the "Call Scotty" ability.
 * Functionality:
 *  - Unique ability granted by finding the Combadge.
 *  - Beams all enemies (Tribbles) into deep space, instantly defeating them.
 */
using FinalProject.Classes;
using FinalProject.Interfaces;

namespace FinalProject.Players
{
    public class CallScottyAbility : IAbility
    {
        public string Name => "Call Scotty";
        public string Description => "Beam all Tribbles into deep space! 🌌";
        public int Cooldown => 2; // Reduced cooldown so 3 attempts is feasible
        public int CooldownRemaining { get; set; }

        public string Activate(ICombatant user, ICombatant target, ICombatContext context)
        {
            // Verify we are fighting Tribbles
            bool fightingTribbles = context.Enemies.Any(e => e is FinalProject.Classes.Tribble);
            if (!fightingTribbles)
            {
                context.AppendLog("Scotty: 'I can't get a lock on those signals, Captain! It only works on Tribbles!' 🚫");
                return null; // Signal to CombatEngine that the ability failed/aborted
            }

            if (user is Character player)
            {
                player.ScottyCallCount++;
                context.AppendLog("You tap your Combadge... 'Scotty, beam them up!'");

                if (player.ScottyCallCount == 1)
                {
                    context.AppendLog("Scotty: 'I canna do it Captain! The interference is too thick!' ⚠️");
                    return " "; 
                }
                else if (player.ScottyCallCount == 2)
                {
                    context.AppendLog("Scotty: 'I'm givin' her all she's got, but the transporters are overheating!' 🔥");
                    return " ";
                }

                // Third time's the charm
                context.AppendLog("Scotty: 'Aye Captain! Locking on now!'");
                context.AppendLog("⚡ A shimmering transporter beam engulfs the room! ⚡");

                foreach (var enemy in context.Enemies)
                {
                    if (enemy.IsAlive())
                    {
                        enemy.CurrentHP = 0;
                        context.AppendLog($"{((Character)enemy).Name} dematerializes into deep space! 🌌");
                    }
                }
                
                context.AppendLog("Combat ended. Thanks Scotty!");
                
                context.EndCombat();
                return " ";
            }
            return "Failed to call Scotty.";
        }
    }
}
