using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Crafting/Gun", order = 1)]
public class WeaponData : ScriptableBase
{
    public float BaseDamage;
    public float FireRate;
    public SprayPattern ShootPattern;
}