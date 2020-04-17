using System.Collections;
using UnityEngine;

// Base class for all the bullets, holds the functinos to destroy themselfes
public abstract class BulletBase : MonoBehaviour, IBullet 
{
   protected const float SELF_DESTRUCT_TIME = 5f;
   public float BulletSpeed { set; get;} = 8f;

   public abstract void Shoot(Vector2 direction);    


    protected IEnumerator initSelfDestructionSequence()
    {
        yield return new WaitForSeconds(SELF_DESTRUCT_TIME);
        Destroy(gameObject);
        yield return null;
    }

    protected void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag.Equals(Tags.ENEMY))
        {
            IHealthManager hpMan = other.GetComponent<IHealthManager>();
            // todo change for real dmg value
            hpMan.LoseHealth(20);
            Destroy(gameObject);    
        }
    }
}