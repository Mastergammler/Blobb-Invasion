using System.Collections;
using UnityEngine;
using BlobbInvasion.Utilities;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{
    [RequireComponent(typeof(LineRenderer))]
    public class VoltageBigRay : BulletBase
    {

        private const float TIME_UNTIL_DESTROY = 0.1f;

        //###############
        //##  MEMBERS  ##
        //###############
        private LineRenderer mLine;

        //################
        //##    MONO    ##
        //################

        private void Awake()
        {
            mLine = GetComponent<LineRenderer>();
        }

        public override void Shoot(Vector2 direction, float damage)
        {
            mBulletDamage = damage;
            direction.Normalize();
            checkHitEnemy(direction, damage);
            StartCoroutine(DestroyAfterOneFrame());
        }


        private void checkHitEnemy(Vector2 direction, float damage)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            mLine.SetPosition(0, transform.position);

            if (hit && hit.transform.tag.Equals(Tags.ENEMY))
            {
                IHealthManager enemyHp = hit.transform.GetComponent<IHealthManager>();
                enemyHp.LoseHealth(damage);

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                mLine.SetPosition(1, hit.transform.position);
            }
            else
            {
                mLine.SetPosition(1, calculateEvenEndPoint(direction));
            }
        }

        private Vector2 calculateEvenEndPoint(Vector2 direction)
        {
            Vector2 directionVector = direction * 50;
            Vector2 shootDirection = new Vector2(transform.position.x, transform.position.y);
            return shootDirection + directionVector;
        }

        private IEnumerator DestroyAfterOneFrame()
        {
            yield return new WaitForSeconds(TIME_UNTIL_DESTROY);
            Destroy(gameObject);
            yield return null;
        }

    }
}