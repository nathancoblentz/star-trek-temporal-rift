using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace FinalProject.Classes
{
    public class RomulanScout : Enemy
    {
        public RomulanScout(Image img = null)
            : base(
                  name: "Romulan Scout",
                  hp: 78,
                  str: 20,
                  dex: 12,
                  def: 7,
                  wis: 10,
                  abilities: new List<IAbility>(),
                  img: img
            )
        {
        }

        public override void OnTurnStart(FinalProject.GameLogic.CombatEngine engine)
        {
            // Simple logic: 20% chance to cloak (buff evasion/dex)
            if (new System.Random().Next(0, 100) < 20)
            {
                engine.Form.AppendLog("The Romulan Scout flickers in and out of cloak! (+3 Dexterity)");
                this.Dexterity += 3;
            }
        }
    }
}
