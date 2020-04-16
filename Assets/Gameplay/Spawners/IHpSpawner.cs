using System;

public interface IHpSpawner
{
    event EventHandler OnItemCollected;
    void Spawn();
}