using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Player
{
    public interface IWeaponSystem
    {
        void Shoot(Vector2 direction);
    }
}