using System.Collections;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Character;
using BlobbInvasion.Gameplay.Character.Enemies;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{


    // Base class for all the bullets, holds the functinos to destroy themselfes
    public abstract class BulletBase : MonoBehaviour, IBullet
    {
        protected const float SELF_DESTRUCT_TIME = 5f;
        protected const float TIME_TO_DESTROY_AFTER_HIT = 1.5f;
        public float BulletSpeed { private set; get; } = 8f;

        [SerializeField][Range(0,1)]
        private float BulletDmgAfterPenetration = 1f;
        public float Penetration => BulletDmgAfterPenetration;

        protected float mBulletDamage;

        public abstract void Shoot(Vector2 direction, float damage);
        protected IEnumerator initSelfDestructionSequence()
        {
            yield return new WaitForSeconds(SELF_DESTRUCT_TIME);
            Destroy(gameObject);
            yield return null;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals(Tags.ENEMY))
            {
                IHealthManager hpMan = other.GetComponent<IHealthManager>();
                // todo change for real dmg value
                hpMan.LoseHealth(mBulletDamage);
                DestroyWithDelay();
            }
            else if (other.tag.Equals(Tags.PROTECTOR))
            {
                IProtector prot = other.GetComponent<IProtector>();
                prot.BulletHit(mBulletDamage,this);
                BulletSpeed *= prot.SpeedReductionMod;
                mBulletDamage *= prot.DamageReductionMod;
                DestroyWithDelay();
            }
        }

        protected void DestroyWithDelay()
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartCoroutine(ActuallyDestroyObject());
        }

        protected IEnumerator ActuallyDestroyObject()
        {
            yield return new WaitForSeconds(TIME_TO_DESTROY_AFTER_HIT);
            Destroy(gameObject);
        }
    }
}