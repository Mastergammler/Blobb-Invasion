using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BlobbInvasion.Environment.SpawnSystems
{
    public class HpSpawnerSystem : MonoBehaviour
    {
        //################
        //##  INSPCTOR  ##
        //################

        public Transform PlayerPosition;
        public Transform[] SpawnPoints;

        public int MinAvailableHpPads;
        public int MaxAvailableHpPads;
        [Range(0, 1)]
        public float DefaultSpawnChance;

        public GameObject HpSpawnerPrefab;


        //#################
        //##  CONSTANTS  ##
        //#################

        private const float TIME_TILL_RE_SPAWN = 15f;
        private const float MAX_SPAWN_MOD_CAP = 3f;
        private const float SPAWN_MOD_BALANCED_AT = 25f;

        //###############
        //##  MEMBERS  ##
        //###############

        private List<Transform> mOccupiedSpawners = new List<Transform>();
        private List<Transform> mIdleSpawners = new List<Transform>();

        private int mCurrentlyActiveSpawners;
        private bool mStopSpawning = false;

        //############
        //##  MONO  ##
        //############

        private void Start()
        {
            foreach (Transform t in SpawnPoints)
            {
                GameObject spawner = Instantiate(HpSpawnerPrefab, t.position, Quaternion.identity);
                mIdleSpawners.Add(spawner.transform);
                spawner.GetComponent<IHpSpawner>().OnItemCollected += SpawnerActivated;
            }

            InitSpawnCycle();
        }

        //###############
        //##  METHODS  ##
        //###############

        private void InitSpawnCycle()
        {
            InvokeRepeating("SpawnHealthPoints", 0, TIME_TILL_RE_SPAWN);
        }

        private void SpawnHealthPoints()
        {
            Spawn();
        }

        private void Spawn()
        {
            List<Transform> mChangeList = new List<Transform>();

            foreach (Transform t in mIdleSpawners)
            {
                float distanceToPlayer = Vector3.Distance(t.position, PlayerPosition.position);
                if (trySpawnFor(t, distanceToPlayer))
                {
                    mChangeList.Add(t);
                }
            }

            // cant change the list while iterating through it
            foreach (Transform t in mChangeList)
            {
                mOccupiedSpawners.Add(t);
                mIdleSpawners.Remove(t);
            }

            if (MinAvailableHpPads > mCurrentlyActiveSpawners) Spawn();
        }


        private bool trySpawnFor(Transform transform, float spawnChance)
        {
            if (mCurrentlyActiveSpawners >= MaxAvailableHpPads) return false;

            float rnd = UnityEngine.Random.Range(0f, 1f);
            if (rnd <= spawnChance)
            {
                transform.GetComponent<IHpSpawner>().Spawn();
                mCurrentlyActiveSpawners++;
                return true;
            }
            return false;
        }

        private float calculateSpawnChance(float playerDistance)
        {
            float spawnMod = SPAWN_MOD_BALANCED_AT / playerDistance;
            if (spawnMod > MAX_SPAWN_MOD_CAP) spawnMod = MAX_SPAWN_MOD_CAP;

            return spawnMod * DefaultSpawnChance;
        }

        //#################
        //##  AUXILIARY  ##
        //#################

        private void SpawnerActivated(object sender, SpawnerEventArgs e)
        {
            mIdleSpawners.Add(e.Position);
            mOccupiedSpawners.Remove(e.Position);
            mCurrentlyActiveSpawners--;
        }


    }
}