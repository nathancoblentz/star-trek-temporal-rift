using FinalProject.Classes;
using FinalProject.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace FinalProject.Classes
{
    public class Bat : Enemy
    {
        public Bat(Image img = null)
            : base(
                  name: "Space Bat",
                  hp: 30,
                  str: 8,
                  dex: 15,
                  def: 2,
                  wis: 5,
                  abilities: new List<IAbility>(),
                  img: img
            )
        {
        }

        public override void OnTurnStart(FinalProject.GameLogic.CombatEngine engine)
        {
             if (new System.Random().NextDouble() < 0.3)
            {
                engine.Form.AppendLog($"{Name} screeches loudly! 🦇");
            }
        }
    }
}
