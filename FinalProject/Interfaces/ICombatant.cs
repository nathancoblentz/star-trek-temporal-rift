namespace FinalProject.Interfaces
{
    // Combines attacking and being attacked, plus health management
    public interface ICombatant : IAttackable, IAttackableTarget
    {
        string Name { get; }
        int CurrentHP { get; }
        int MaxHP { get; }
        bool IsAlive();
        int Heal(int healAmount);
        List<IAbility> Abilities { get; }
    }
}
