using UnityEngine;

[CreateAssetMenu(fileName = "new Bullet ", menuName = "Crafting/Bullet", order = 2)]
public class BulletData : ScriptableBase 
{
    public Sprite CoreSpecificArt;
    public float DamageMod;
    public float FireRateMod;   
    public BulletType BulletType;
    public IBulletBehaviour BulletBehaviour;
    public IBulletAnimation BulletAnimation;
}

public enum BulletType
{
    DEFAULT,BIG,BOMB,SCRAPINOL
}