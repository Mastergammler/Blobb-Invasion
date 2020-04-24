using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using BlobbInvasion.Utilities;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BigBulletProjectile : BulletBase
    {
        //#################
        //##  CONSTANTS  ##
        //#################

        private const float DESTROY_DELAY = 0.125f;
        private bool mIsAboutToBeDestroyed = false;

        //############
        //##  MONO  ##
        //############

        public override void Shoot(Vector2 direction, float damage)
        {
            mBulletDamage = damage;
            direction.Normalize();
            GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;
            StartCoroutine(initSelfDestructionSequence());
        }


        private new void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals(Tags.ENEMY))
            {
                IHealthManager hpMan = other.GetComponent<IHealthManager>();
                // todo change for real dmg value
                hpMan.LoseHealth(mBulletDamage);
                if (!mIsAboutToBeDestroyed)
                {
                    StartCoroutine(DestoryOfterDelay());
                }
            }
        }


        private IEnumerator DestoryOfterDelay()
        {
            mIsAboutToBeDestroyed = true;
            yield return new WaitForSeconds(DESTROY_DELAY);
            DestroyWithDelay();
            yield return null;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //base.OnTriggerEnter2D(other);
        }
    }
}