using System;
using UnityEngine;

public interface IHpSpawner
{
    event EventHandler<SpawnerEventArgs> OnItemCollected;
    void Spawn();
}

public class SpawnerEventArgs : EventArgs
{
    public Transform Position;
}