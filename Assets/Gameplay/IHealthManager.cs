public interface IHealthManager
{
    void Init();
    void GainHealth(float amount);
    void LoseHealth(float amount);
    void AdjustColorBasedOnHp(bool adjust);

    float GetCurrentHealth();
    float GetCurrentHealthPercentage();


}