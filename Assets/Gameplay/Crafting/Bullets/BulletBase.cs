using System.Collections;
using UnityEngine;

// Base class for all the bullets, holds the functinos to destroy themselfes
public abstract class BulletBase : MonoBehaviour, IBullet 
{
   protected const float SELF_DESTRUCT_TIME = 5f;
   protected const float TIME_TO_DESTROY_AFTER_HIT = 1.5f;
   public float BulletSpeed { set; get;} = 8f;

   public abstract void Shoot(Vector2 direction);    


    protected IEnumerator initSelfDestructionSequence()
    {
        yield return new WaitForSeconds(SELF_DESTRUCT_TIME);
        Destroy(gameObject);
        yield return null;
    }

    protected void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag.Equals(Tags.ENEMY))
        {
            IHealthManager hpMan = other.GetComponent<IHealthManager>();
            // todo change for real dmg value
            hpMan.LoseHealth(20);
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