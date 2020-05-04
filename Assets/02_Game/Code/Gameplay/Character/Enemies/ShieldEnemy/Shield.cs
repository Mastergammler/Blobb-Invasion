using System;
using UnityEngine;
using BlobbInvasion.Gameplay.Items.Crafting.Bullets;
using BlobbInvasion.Gameplay.Effects;
using BlobbInvasion.Utilities;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{

    public delegate void OnPlayerCollision();

    [RequireComponent(typeof(Collider2D))]
    public class Shield : MonoBehaviour, IProtector
    {
        
        [SerializeField]
        private float ShieldHealth = 250;
        [SerializeField][Range(0,1)]
        private float ReductionModifier;
        [SerializeField][Range(0,1)]
        private float SpeedReductionModifier;

        private ISpriteMaterialChanger mMaterialChanger;
        private RobotEnemyMaster.OnShieldDestroyed mShieldDestroyedCallback;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMaterialChanger = GetComponent<ISpriteMaterialChanger>();
        }

        //###############
        //##  METHODS  ##
        //###############

        private void CalculateDamage(float rawDmg,IBullet bullet)
        {
            Type bulletType = bullet.GetType();

            if(bulletType == typeof(DefaultBullet) || bulletType == typeof(BigBulletProjectile))
            {
                GetDmg(rawDmg * ReductionModifier * bullet.Penetration);
                mMaterialChanger.ChangeMaterial();
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals(Tags.PLAYER))
            {
                CollisionWithPlayer?.Invoke();
            }
        }

        public void ShieldDestroyedCallback(RobotEnemyMaster.OnShieldDestroyed action)
        {
            mShieldDestroyedCallback = action;
        }

        private void OnDestroy()
        {
            mShieldDestroyedCallback?.Invoke();
        }


        //*******************
        //**  I PROTECTOR  **
        //*******************

        public event OnPlayerCollision CollisionWithPlayer;

        public float DamageReductionMod => ReductionModifier;
        public float Hitpoints => ShieldHealth;
        public float SpeedReductionMod => SpeedReductionModifier; 
        public void BulletHit(float damage,IBullet bullet)
        {
            CalculateDamage(damage,bullet);
        }
    }
}