using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace FinalProject.Classes
{
    public class Q : Enemy
    {
        private static System.Random rng = new System.Random();

        public Q(Image img = null)
            : base(
                  name: "Q",
                  hp: 500, // High HP, he's a god-like being
                  str: 15,  // Increased damage, he's getting bored
                  dex: 20, // Hard to hit
                  def: 20, // Hard to hurt
                  wis: 99,
                  abilities: new List<IAbility>(),
                  img: img
            )
        {
        }

        public override void OnTurnStart(FinalProject.GameLogic.CombatEngine engine)
        {
            int roll = rng.Next(1, 6);
            switch (roll)
            {
                case 1:
                    engine.Form.AppendLog("Q snaps his fingers and a mariachi band appears! 🎺");
                    engine.Form.AppendLog("The trumpet blast hurts your ears! (10 Sonic Damage)");
                    engine.Form.CurrentPlayer.TakeDamage(10);
                    break;
                case 2:
                    engine.Form.AppendLog("Q changes your uniform into Robin Hood cosplay. 'Fetching!'");
                    break;
                case 3:
                    engine.Form.AppendLog("Q yawns. 'Is this the best humanity has to offer?'");
                    break;
                case 4:
                    engine.Form.AppendLog("Q smiles mischievously. 'Let's see how you fare elsewhere.'");
                    engine.TeleportPlayer();
                    break;
                case 5:
                    engine.Form.AppendLog("Q offers you a cigar. It explodes in your face! 💥");
                    engine.Form.AppendLog("That really hurt! (40 Damage)");
                    engine.Form.CurrentPlayer.TakeDamage(40);
                    break;
            }
            
        }

        public override string GetDefeatMessage()
        {
            return "Q laughs. 'A temporary setback, mon capitaine!' He vanishes in a flash of light. ✨";
        }
    }
}
