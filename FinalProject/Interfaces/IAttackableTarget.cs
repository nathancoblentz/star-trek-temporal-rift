namespace FinalProject.Interfaces
{
    public interface IAttackableTarget
    {
        // Receives damage and returns the effective damage taken
        int TakeDamage(int amount);
    }
}
