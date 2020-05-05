using System;
using UnityEngine;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Gameplay;

namespace BlobbInvasion.Environment.SpawnSystems
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        public float SpawnRadius;
        public event EnemyDied OnEnemyKilled;

        //###############
        //##  MEMBERS  ##
        //###############

        private int mSpawnedAliveEnemyCount = 0;

        private IGOFactory mFactory;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mFactory = MasterFactory.Instance.GetFactory(FactoryType.ENEMY_SHIELD_ROBOT);
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void Spawn()
        {
            GameObject spawnedObject  = mFactory.CreateGameObject(GetSpawnPoint());
            mSpawnedAliveEnemyCount++;

            IObservable obs = spawnedObject.GetComponent<IObservable>();
            if (obs == null) Debug.LogWarning("Target Class is not a observable!");
            else obs.OnObservableAction += WasKilled;
        }

        public void WasKilled()
        {
            mSpawnedAliveEnemyCount--;
            OnEnemyKilled?.Invoke(mSpawnedAliveEnemyCount == 0);
        }

        private Vector3 GetSpawnPoint()
        {
            Vector2 rndDirection = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            rndDirection.Normalize();

            return new Vector3(rndDirection.x * SpawnRadius + transform.position.x, rndDirection.y * SpawnRadius + transform.position.y);
        }
    }
}