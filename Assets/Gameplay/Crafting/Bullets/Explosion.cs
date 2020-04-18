using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IExplosion
{
    private const float DESTORY_DELAY = 2f;
    private const float IMPACT_DELAY = 0.2f;
    private float explosionDamage;

    void Start()
    {
        StartCoroutine(destroyAfterDelay());
    }

    public void SetDamage(float damage)
    {
        explosionDamage = damage;
    }

    private IEnumerator destroyAfterDelay()
    {
        yield return new WaitForSeconds(IMPACT_DELAY);
        GetComponent<PointEffector2D>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(DESTORY_DELAY);
        Destroy(gameObject);
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag.Equals(Tags.ENEMY))
        {
            IHealthManager hm = other.GetComponent<IHealthManager>();
            if(hm != null) hm.LoseHealth(explosionDamage);
        }   
    }

}
