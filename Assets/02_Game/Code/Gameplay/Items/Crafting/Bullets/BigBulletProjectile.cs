using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Character;

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

        protected new void DestroyWithDelay()
        {
            if (!mIsAboutToBeDestroyed)
            {
                StartCoroutine(DestoryOfterDelay());
            }
        }


        private IEnumerator DestoryOfterDelay()
        {
            mIsAboutToBeDestroyed = true;
            yield return new WaitForSeconds(DESTROY_DELAY);
            DestroyWithDelay();
            yield return null;
        }
    }
}