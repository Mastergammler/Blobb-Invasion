using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour 
{
    //################
    //##  INSPCTOR  ##
    //################

    public Transform PlayerPosition;
    public Transform[] SpawnPoints;

    public int MinAvailableEnemies;
    public int MaxAvailbaleEnemies;
    [Range(0,1)]
    public float DefaultSpawnChance;
    public float PlayerDeadZone;
    public bool PairSpawnMode = false;

    public GameObject EnemySpawnerPrefab;


    //#################
    //##  CONSTANTS  ##
    //#################

    private const float TIME_TILL_RE_SPAWN = 30f;
    private const float MAX_SPAWN_MOD_CAP = 3.5f;
    private const float SPAWN_MOD_BALANCED_AT = 25f;

    //###############
    //##  MEMBERS  ##
    //###############

    private List<Transform> mSpawnerList = new List<Transform>();
    private int mCurrentlyActiveMobs;
    private bool mStopSpawning = false;

    //############
    //##  MONO  ##
    //############

    private void Start() 
    {
        foreach(Transform t in SpawnPoints)
        {
            GameObject spawner = Instantiate(EnemySpawnerPrefab,t.position,Quaternion.identity);
            mSpawnerList.Add(spawner.transform);
            spawner.GetComponent<IEnemySpawner>().OnEnemyKilled += SpawnerActivated;
        }   

        InitSpawnCycle();
    }

    //###############
    //##  METHODS  ##
    //###############

    private void InitSpawnCycle()
    {
        InvokeRepeating("SpawnHealthPoints",0,TIME_TILL_RE_SPAWN);
    }

    private void SpawnHealthPoints()
    {
        Spawn();
    }

    private void Spawn()
    {
        List<Transform> mChangeList = new List<Transform>();

        foreach(Transform t in mSpawnerList)
        {
            float distanceToPlayer = Vector3.Distance(t.position,PlayerPosition.position);
            if(distanceToPlayer <= PlayerDeadZone) continue;

             if (trySpawnFor(t,distanceToPlayer))
             {
                 //mChangeList.Add(t);
             }
        }

        // cant change the list while iterating through it
        foreach(Transform t in mChangeList) 
        {
            //mOccupiedSpawners.Add(t);
            //mIdleSpawners.Remove(t);
        }

        if(MinAvailableEnemies > mCurrentlyActiveMobs) Spawn();
    }


    private bool trySpawnFor(Transform transform,float spawnChance)
    {
        if(mCurrentlyActiveMobs >= MaxAvailbaleEnemies) return false;

        float rnd = UnityEngine.Random.Range(0f,1f);
        if(rnd <= spawnChance)
        {
            spawnTheEnemy(transform);
            if(PairSpawnMode) spawnTheEnemy(transform);
            return true;
        }
        return false;
    }

    private void spawnTheEnemy(Transform transform)
    {
            transform.GetComponent<IEnemySpawner>().Spawn();
            mCurrentlyActiveMobs ++;
    }

    private float calculateSpawnChance(float playerDistance)
    {
        float spawnMod = SPAWN_MOD_BALANCED_AT / playerDistance;
        if(spawnMod > MAX_SPAWN_MOD_CAP) spawnMod = MAX_SPAWN_MOD_CAP;

        return spawnMod * DefaultSpawnChance;
    }

    //#################
    //##  AUXILIARY  ##
    //#################

    private void SpawnerActivated(bool allEnemiesDead)
    {
        mCurrentlyActiveMobs --;
    }


}