using System;
using UnityEngine;
using BlobbInvasion.Gameplay.Items.Crafting.Bullets;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    [RequireComponent(typeof(Collider2D))]
    public class Shield : MonoBehaviour, IProtector
    {
        
        [SerializeField]
        private float ShieldHealth = 250;
        [SerializeField][Range(0,1)]
        private float ReductionModifier;
        [SerializeField][Range(0,1)]
        private float SpeedReductionModifier;

        //###############
        //##  METHODS  ##
        //###############

        private void CalculateDamage(float rawDmg,IBullet bullet)
        {
            Type bulletType = bullet.GetType();

            if(bulletType == typeof(DefaultBullet) || bulletType == typeof(BigBulletProjectile))
            {
                GetDmg(rawDmg * ReductionModifier * bullet.Penetration);
            }
            else 
            {
                // do nothing :)
            }
        }

        private void GetDmg(float damage)
        {
            ShieldHealth -= damage;
            if(ShieldHealth <= 0) DestroyShield();
        }

        private void DestroyShield()
        {
            Destroy(gameObject);
        }


        //*******************
        //**  I PROTECTOR  **
        //*******************

        public float DamageReductionMod => ReductionModifier;
        public float Hitpoints => ShieldHealth;
        public float SpeedReductionMod => SpeedReductionModifier; 
        public void BulletHit(float damage,IBullet bullet)
        {
            Debug.Log("Bullet hit confirmed");
            CalculateDamage(damage,bullet);
        }
    }
}