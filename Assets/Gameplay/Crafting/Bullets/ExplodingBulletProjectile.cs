using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class ExplodingBulletProjectile : BulletBase
{
    //#################
    //##  CONSTANTS  ##
    //#################

    private const float DESTROY_DELAY = 0.05f;
    private bool mIsAboutToBeDestroyed = false;
    public GameObject Explosion;
    public float TimeUntilExplosion = 1f;
    private bool mFirstHit = true;

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
        if(other.tag.Equals(Tags.ENEMY) && mFirstHit)
        {
            mFirstHit = false;
            if(! mIsAboutToBeDestroyed)
            {
                StartCoroutine(DestoryOfterDelay());
            }
        }
    }


    protected new IEnumerator initSelfDestructionSequence()
    {
        yield return new WaitForSeconds(TimeUntilExplosion);
        DestroyWithDelay();
        yield return null;
    }

    protected new void DestroyWithDelay()
    {
        if(mIsAboutToBeDestroyed) return;
        mIsAboutToBeDestroyed = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        createExplosion();
        StartCoroutine(ActuallyDestroyObject());
    }

    private IEnumerator DestoryOfterDelay()
    {
        yield return new WaitForSeconds(DESTROY_DELAY);
        DestroyWithDelay();
        yield return null;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        //base.OnTriggerEnter2D(other);
    }

    private void createExplosion()
    {
        GameObject explosion = Instantiate(Explosion,transform.position,Quaternion.identity);
        IExplosion exp = explosion.GetComponent<IExplosion>();
        exp.SetDamage(mBulletDamage);
    }
}