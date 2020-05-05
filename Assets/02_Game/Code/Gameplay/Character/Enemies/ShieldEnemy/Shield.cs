using System;
using UnityEngine;
using BlobbInvasion.Gameplay.Items.Crafting.Bullets;
using BlobbInvasion.Gameplay.Effects;
using BlobbInvasion.Utilities;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    [RequireComponent(typeof(Collider2D))]
    public class Shield : MonoBehaviour, IProtector
    {
        [SerializeField]
        private float ShieldHealth = 250;
        [SerializeField]
        [Range(0, 1)]
        private float ReductionModifier;
        [SerializeField]
        [Range(0, 1)]
        private float SpeedReductionModifier;

        private ISpriteMaterialChanger mMaterialChanger;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMaterialChanger = GetComponent<ISpriteMaterialChanger>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals(Tags.PLAYER))
            {
                OnPlayerCollision?.Invoke();
            }
        }

        private void OnDestroy()
        {
            OnShieldDestroyed?.Invoke();
        }

        //#################
        //##  PROTECTOR  ##
        //#################
        public void BulletHit(float damage, IBullet bullet)
        {
            CalculateDamage(damage, bullet);
        }

        public float DamageReductionMod => ReductionModifier;
        public float Hitpoints => ShieldHealth;
        public float SpeedReductionMod => SpeedReductionModifier;

        //###############
        //##  METHODS  ##
        //###############

        private void CalculateDamage(float rawDmg, IBullet bullet)
        {
            Type bulletType = bullet.GetType();

            if (bulletType == typeof(DefaultBullet) || bulletType == typeof(BigBulletProjectile))
            {
                DamageShieldBy(rawDmg * ReductionModifier * bullet.Penetration);
                mMaterialChanger.ChangeMaterial();
            }
            // else don't effect the bullets
        }

        private void DamageShieldBy(float damage)
        {
            ShieldHealth -= damage;
            if (ShieldHealth <= 0) DestroyShield();
        }

        private void DestroyShield()
        {
            //todo sound effects, particles etc
            Destroy(gameObject);
        }

        //#################
        //##  ACCESSORS  ##
        //#################
        public event Action OnPlayerCollision;
        public event Action OnShieldDestroyed;


    }
}