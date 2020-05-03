
namespace BlobbInvasion.Gameplay.Character
{
    // Responsible to manage health of all kinds of creatures
    public interface IHealthManager
    {
        void Init();
        // Summary:
        // Adds health to the current creature
        void GainHealth(float amount);
        void LoseHealth(float amount);
        void AdjustColorBasedOnHp(bool adjust);

        float GetCurrentHealth();
        float GetCurrentHealthPercentage();


    }
}