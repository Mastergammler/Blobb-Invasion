
using UnityEngine;

namespace BlobbInvasion.Gameplay.Items.Crafting
{
    [CreateAssetMenu(fileName = "New Weapon Pattern", menuName = "Crafting/Weapon/Pattern", order = 1)]
    public class SprayPattern : ScriptableObject
    {
        public Vector2[] Pattern;

        // todo functions for manipulating it
    }
}