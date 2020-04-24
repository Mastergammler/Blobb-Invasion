
using UnityEngine;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{
    public interface IBullet
    {
        float BulletSpeed { set; get; }
        void Shoot(Vector2 direction, float damage);
    }
}