using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Core", menuName = "Crafting/Core", order = 0)]
public class CoreData : ScriptableObject 
{
    public float BaseDmgMod;
    public float BaseFireRateMod;

    public Color ColorDark;
    public Color ColorBright;

    public BulletData[] Bullets; 
}