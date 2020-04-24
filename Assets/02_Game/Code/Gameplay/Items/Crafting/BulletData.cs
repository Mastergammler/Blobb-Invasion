using UnityEngine;

[CreateAssetMenu(fileName = "new Bullet ", menuName = "Crafting/Bullet", order = 2)]
namespace BlobbInvasion. 
{
public class
 BulletData : ScriptableBase 
{
    public Sprite CoreSpecificArt;
    public float DamageMod;
    public float FireRateMod;   
    public BulletType BulletType;
    public AudioClip BulletSound;
    public IBulletBehaviour BulletBehaviour;
    public IBulletAnimation BulletAnimation;
    public GameObject BulletPrefab;
}

public enum BulletType
{
    DEFAULT,BIG,BOMB,SCRAPINOL
}