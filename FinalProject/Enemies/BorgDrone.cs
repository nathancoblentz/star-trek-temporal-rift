using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace FinalProject.Classes
{
    public class BorgDrone : Enemy
    {
        public BorgDrone(Image img = null)
            : base(
                  name: "Borg Drone",
                  hp: 40,
                  str: 8,
                  dex: 5,
                  def: 10,
                  wis: 10,
                  abilities: new List<IAbility>(),
                  img: img
            )
        {
        }

        public override void OnTurnStart(FinalProject.GameLogic.CombatEngine engine)
        {
            // Simple logic: 20% chance to adapt (buff defense)
            if (new System.Random().Next(0, 100) < 20)
            {
                engine.Form.AppendLog("The Borg Drone adapts to your attacks! (+2 Defense)");
                this.Defense += 2;
            }
        }
    }
}
