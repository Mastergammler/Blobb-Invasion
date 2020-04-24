

public delegate void EnemyDied(bool allEnemies);

public interface IEnemySpawner
{
    event EnemyDied OnEnemyKilled;
    void Spawn();
}