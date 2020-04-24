using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobbInvasion.Gameplay.Effects;

namespace BlobbInvasion.Gameplay.Character.Enemies
{
    public class BasicHealthSystem : MonoBehaviour, IHealthManager
    {
        public float MaximumHp = 100;

        //###############
        //##  MEMBERS  ##
        //###############

        private float mCurrentHp;
        private ISpriteMaterialChanger mSpriteMaterialChanger;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mCurrentHp = MaximumHp;
            mSpriteMaterialChanger = GetComponent<ISpriteMaterialChanger>();
        }

        //########################
        //##  I HELATH MANAGER  ##
        //########################

        public void Init() { }

        public void GainHealth(float amount)
        {
            mCurrentHp += amount;
            if (mCurrentHp > MaximumHp) mCurrentHp = MaximumHp;
        }

        public void LoseHealth(float amount)
        {
            if (mSpriteMaterialChanger != null)
            {
                mSpriteMaterialChanger.ChangeMaterial();
            }
            mCurrentHp -= amount;
            CheckForDeath();
        }

        public void AdjustColorBasedOnHp(bool adjust)
        {
            // do nothing, not supported
        }

        public float GetCurrentHealth() { return mCurrentHp; }
        public float GetCurrentHealthPercentage() { return mCurrentHp / MaximumHp; }

        //#################
        //##  AUXILIARY  ##
        //#################

        private void CheckForDeath()
        {
            if (mCurrentHp <= 0) Death();
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}