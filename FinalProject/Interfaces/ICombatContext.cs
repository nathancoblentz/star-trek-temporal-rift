using System.Collections.Generic;
using FinalProject.Classes;

namespace FinalProject.Interfaces
{
    public interface ICombatContext
    {
        List<Enemy> Enemies { get; }
        void AppendLog(string message);
        void EndCombat();
        void UpdateUI();
    }
}
