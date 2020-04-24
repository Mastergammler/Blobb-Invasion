using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using BlobbInvasion.Utilities;

public class VoltageExplosion : MonoBehaviour, IExplosion
{
    private const float DESTORY_DELAY = 2.5f;
    private const float IMPACT_DELAY = 0.2f;
    private float explosionDamage;
    public float SurgeTime = 2f;
    public float TickDmgTime = 0.2f;

    private float mTimeSinceLastTick = 0;
    private bool mDoTickDmg = true;

    void Start()
    {
        StartCoroutine(destroyAfterDelay());
    }

    private void FixedUpdate()
    {
        mTimeSinceLastTick += Time.fixedDeltaTime;
        if(mTimeSinceLastTick > TickDmgTime)
        {
            mDoTickDmg = true;
            mTimeSinceLastTick %= TickDmgTime;
        }
        else
        {
            mDoTickDmg = false;
        }

    }

    public void SetDamage(float damage)
    {
        explosionDamage = damage;
    }

    private IEnumerator destroyAfterDelay()
    {
        yield return new WaitForSeconds(SurgeTime);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(! mDoTickDmg) return;

        DmgEnemy(other);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DmgEnemy(other);
    }


    private void DmgEnemy(Collider2D other)
    {
        if(other.tag.Equals(Tags.ENEMY))
        {
            IHealthManager hm = other.GetComponent<IHealthManager>();
            if(hm != null) hm.LoseHealth(explosionDamage);
        }   
    }

}