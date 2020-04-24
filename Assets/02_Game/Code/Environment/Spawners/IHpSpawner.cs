using System;
using UnityEngine;

namespace BlobbInvasion.Environment.SpawnSystems
{
    public interface IHpSpawner
    {
        event EventHandler<SpawnerEventArgs> OnItemCollected;
        void Spawn();
    }

    public class SpawnerEventArgs : EventArgs
    {
        public Transform Position;
    }

}