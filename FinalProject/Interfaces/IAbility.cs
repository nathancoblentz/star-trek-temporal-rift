using FinalProject.Classes;

namespace FinalProject.Interfaces
{
    public interface IAbility
    {
        string Name { get; }
        string Description { get; }

        int Cooldown { get; } // total cooldown
        int CooldownRemaining { get; set; } // tracks turns left

        void StartCooldown() => CooldownRemaining = Cooldown;
        void ReduceCooldown() { if (CooldownRemaining > 0) CooldownRemaining--; }

        string Activate(ICombatant user, ICombatant target, ICombatContext context);
    }
}
