using UnityEngine;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class DefaultBullet : BulletBase
    {
        //###############
        //##  MEMBERS  ##
        //###############

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
    }
}