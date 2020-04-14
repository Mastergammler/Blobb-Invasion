using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Crafting/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string Name;
    public string Tooltip;
    public Sprite Art;

    public float BaseDamage;
    public float FireRate;
    public SprayPattern ShootPattern;
}