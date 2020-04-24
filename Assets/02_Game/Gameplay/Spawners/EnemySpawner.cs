using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IEnemySpawner 
{
    public GameObject EnemyToSpawn;
    public event EnemyDied OnEnemyKilled;
    public float SpawnRadius;

    private int mSpawnedAliveEnemyCount = 0;

    public void Spawn()
    {
        var gObj = Instantiate(EnemyToSpawn,GetSpawnPoint(),Quaternion.identity);
        mSpawnedAliveEnemyCount++;

        IObservable obs = gObj.GetComponent<IObservable>();
        if(obs == null) Debug.LogWarning("Target Class is not a observable!");
        else obs.RegisterCallback(WasKilled);
    }

    public void WasKilled()
    {
        mSpawnedAliveEnemyCount--;
        OnEnemyKilled?.Invoke(mSpawnedAliveEnemyCount == 0);
    } 

    private Vector3 GetSpawnPoint()
    {
        Vector2 rndDirection = new Vector2(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f));
        rndDirection.Normalize();

        return new Vector3(rndDirection.x * SpawnRadius + transform.position.x,rndDirection.y * SpawnRadius + transform.position.y);
    }
}