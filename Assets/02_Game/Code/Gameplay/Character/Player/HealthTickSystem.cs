using UnityEngine;
using BlobbInvasion.Core;


// Removes health based on ticks
namespace BlobbInvasion.Gameplay.Character.Player
{
    public class HealthTickSystem : MonoBehaviour, IHealthManager
    {
        [SerializeField]
        private float Health = 100;
        [SerializeField]
        private float HealthLossTick = 3;
        private float MAX_HEALTH;

        [SerializeField]
        private bool AdjustColor = false;
        [SerializeField]
        private Color TargetHue;
        private Color mStartingHue;
        private SpriteRenderer mSpriteRenderer;

        private void Start()
        {
            MAX_HEALTH = Health;
            mSpriteRenderer = GetComponent<SpriteRenderer>();
            mStartingHue = mSpriteRenderer.color;
        }

        private void Update()
        {
            healthTick();
            adjustColor();
            checkForDeath();
        }

        private void healthTick()
        {
            Health -= HealthLossTick * Time.deltaTime;
        }
        private void checkForDeath()
        {
            if (Health <= 0)
            {
                gameObject.active = false;
                MusicManager.Instance.StopMusic();
                GameManager.Instance.PlayerDead();
                Debug.Log("Player died");
            }
        }
        private void adjustColor()
        {
            if (AdjustColor)
            {
                Color adjustedColor = Color.Lerp(TargetHue, mStartingHue, GetCurrentHealthPercentage());
                adjustedColor.a = 1;
                mSpriteRenderer.color = adjustedColor;
            }
        }

        //------------------
        //  IHealthManager
        //------------------

        public void Init()
        {
            Health = MAX_HEALTH;
        }

        public void GainHealth(float amount)
        {
            Health += amount;
            if (Health > MAX_HEALTH) Health = MAX_HEALTH;
        }

        public void LoseHealth(float amount)
        {
            Health -= amount;
            checkForDeath();
        }

        public float GetCurrentHealth()
        {
            return Health;
        }

        public float GetCurrentHealthPercentage()
        {
            return Health / MAX_HEALTH;
        }

        public void AdjustColorBasedOnHp(bool adjust)
        {
            AdjustColor = adjust;
        }
    }
}