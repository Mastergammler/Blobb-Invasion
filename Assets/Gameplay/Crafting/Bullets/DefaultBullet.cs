using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class DefaultBullet : BulletBase
{
    //###############
    //##  MEMBERS  ##
    //###############

    //############
    //##  MONO  ##
    //############

    public override void Shoot(Vector2 direction)
    {
        direction.Normalize();
        GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;
        StartCoroutine(initSelfDestructionSequence());
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}