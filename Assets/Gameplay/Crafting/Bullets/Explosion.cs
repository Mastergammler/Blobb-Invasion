using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private const float DESTORY_DELAY = 2f;
    private const float IMPACT_DELAY = 0.2f;

    void Start()
    {
        StartCoroutine(destroyAfterDelay());
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
            // todo do dmg
        }   
    }

}
