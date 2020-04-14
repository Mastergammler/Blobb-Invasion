using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Core", menuName = "Crafting/Core", order = 0)]
public class CoreData : ScriptableObject 
{
    public string Name;
    public string Tooltip;
    public Sprite Art;

    public float BaseDmgMod;
    public float BaseFireRateMod;

    public Color ColorDark;
    public Color ColorBright;

    public BulletData[] Bullets; 
}