using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using BlobbInvasion.Utilities;

namespace BlobbInvasion. 
{
public class
 Explosion : MonoBehaviour, IExplosion
{

    public bool AddScreenShake = true;
    public float ScreenShakeAmplitude = 1.5f;
    public float FrequencyRamp = 10f;
    public float ShakeTime = 1f;
    public CinemachineVirtualCamera Camera;


    private const float DESTORY_DELAY = 2.5f;
    private const float IMPACT_DELAY = 0.2f;
    private float explosionDamage;

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA).GetComponentInChildren<CinemachineVirtualCamera>();
        StartCoroutine(destroyAfterDelay());
        StartCoroutine(screeShakeWithAutoDisable());
    }

    private IEnumerator screeShakeWithAutoDisable()
    {
        if(Camera != null)
        {
            var noise = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = ScreenShakeAmplitude;
            noise.m_FrequencyGain = FrequencyRamp;
            yield return new WaitForSeconds(ShakeTime);
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
        }
        yield return null;
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
