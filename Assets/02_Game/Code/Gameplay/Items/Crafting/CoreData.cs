using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Core", menuName = "Crafting/Core", order = 0)]
namespace BlobbInvasion. 
{
public class
 CoreData : ScriptableBase 
{
    public float BaseDmgMod;
    public float BaseFireRateMod;

    public Color ColorDark;
    public Color ColorBright;

    public BulletData[] Bullets; 

    public AnimationClip ColorAnimation;
}