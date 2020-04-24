
using UnityEngine;

namespace BlobbInvasion. 
{
public interface
 IBullet
{
    float BulletSpeed { set; get; }
    void Shoot(Vector2 direction, float damage);
}