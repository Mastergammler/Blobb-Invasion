
using UnityEngine;

namespace BlobbInvasion.Gameplay.Items.Crafting.Bullets
{
    public interface IBullet
    {
        // the speed the bullet moves with
        float BulletSpeed { get; }
        // the penetration value of the bullet
        // defines how much dmg the bullet does to any protection object
        float Penetration { get; }


        void Shoot(Vector2 direction, float damage);
    }
}