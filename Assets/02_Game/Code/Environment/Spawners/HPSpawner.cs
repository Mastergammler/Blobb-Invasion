using System.Reflection.Emit;
using UnityEngine;
using System;

namespace BlobbInvasion.Environment.SpawnSystems
{
    public class HPSpawner : MonoBehaviour, IHpSpawner
    {
        public GameObject CollectableToSpawn;
        public event EventHandler<SpawnerEventArgs> OnItemCollected;

        public void Spawn()
        {
            var gObj = Instantiate(CollectableToSpawn, transform.position, Quaternion.identity);
            CollectableBase cb = gObj.GetComponent<CollectableBase>();
            if (cb == null) Debug.LogWarning("Collectable does not extend from collectable base! No callback possible!");
            else cb.RequestCallback(HasBeenCollected);
        }

        public void HasBeenCollected()
        {
            OnItemCollected?.Invoke(this, new SpawnerEventArgs() { Position = transform });
        }

    }
}