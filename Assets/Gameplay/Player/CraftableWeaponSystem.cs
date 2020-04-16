using UnityEngine;

[RequireComponent(typeof(IInventory))]
public class CraftableWeaponSystem : MonoBehaviour,IWeaponSystem
{
    public BulletData DefaultBullet;

    //###############
    //##  MEMBERS  ##
    //###############

    private IInventory mInventory;

    private WeaponData mCurWeapon;
    private BulletData mCurBullet;
    private CoreData mCurCore;

    //----------------
    //  Weapon Data
    //----------------

    private float mFireRate;
    private float mDmgPerShot;

    private float mTimeSinceLastShot = 0f;
    private float mTimeBetweenShots;

    //############
    //##  MONO  ##
    //############

    private void Start() 
    {
        mInventory = GetComponent<IInventory>();
        mInventory.OnActiveItemsChanged += InitNewWeapon;
    }

    //######################
    //## I WEAPON SYSTEM  ##
    //######################

    public void Shoot(Vector2 direction)
    {
        if(canShootAgain())
        {
            
        }
    }

    //###############
    //##  HELPERS  ##
    //###############

    private void InitNewWeapon(object sender, ActiveItemEventArgs e)
    {
        mCurBullet = e.NewBullet;
        mCurWeapon = e.NewWeapon;
        mCurCore = e.NewCore;

        CalculateWeaponValues();
    }

    private void CalculateWeaponValues()
    {
        InitWeaponValue();
        AddCoreValue();
        AddBulletValue();
        CalculateShotReload();
    }

    private void InitWeaponValue()
    {
        if(mCurWeapon == null)
        {
            Debug.Log("No weapon chosen");
            mFireRate = 0;
            mDmgPerShot = 0;
        }
        else
        {
            mFireRate = mCurWeapon.FireRate;
            mDmgPerShot = mCurWeapon.BaseDamage;
        }
    }

    private void AddCoreValue()
    {
        if(mCurCore != null)
        {
            mFireRate *= mCurCore.BaseFireRateMod;
            mDmgPerShot *= mCurCore.BaseDmgMod;
        }
    }

    private void AddBulletValue()
    {
        if(mCurBullet == null) mCurBullet = DefaultBullet;

        mFireRate *= mCurBullet.FireRateMod;
        mDmgPerShot *= mCurBullet.DamageMod;
    }

    private void CalculateShotReload()
    {
        mTimeBetweenShots = 1/mFireRate;
    }

    private bool canShootAgain()
    {
        if(mTimeSinceLastShot >= mTimeBetweenShots)
        {
            mTimeSinceLastShot = 0;
            return true;
        }

        mTimeSinceLastShot += Time.deltaTime;
        return false;
    }

}