using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class DefaultBullet : MonoBehaviour,IBullet 
{
    public float BulletSpeed { set; get;} = 8;

    //###############
    //##  MEMBERS  ##
    //###############

    //############
    //##  MONO  ##
    //############

    public void Shoot(Vector2 direction)
    {
        direction.Normalize();
        GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}