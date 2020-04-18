using UnityEngine;

[RequireComponent(typeof(IInventory),typeof(AudioSource))]
public class CraftableWeaponSystem : MonoBehaviour,IWeaponSystem
{
    public CoreData DefaultCore;
    public WeaponData DefaultWeapon;
    public Transform GunJoint;

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
        mCurWeapon = DefaultWeapon;
        CalculateWeaponValues();
    }

    private void Update() 
    {
        // update time when last shot was
        mTimeSinceLastShot += Time.deltaTime;    
    }

    //######################
    //## I WEAPON SYSTEM  ##
    //######################

    public void Shoot(Vector2 direction)
    {
        if(canShootAgain())
        {
            // bullets in inventory are only for the bullet type, and don't hold the actual values
            BulletData realBullet = mCurCore.Bullets[(int)mCurBullet.BulletType];
            // each gun has a child object that defines outward position
            Transform gunExitPoint = GunJoint.GetChild(0).GetChild(0);
            // todo include multiple bullets (later)

            GameObject prefab = realBullet.BulletPrefab;
            GameObject inst = Instantiate(prefab,gunExitPoint.position,Quaternion.LookRotation(Vector3.forward,direction));
            inst.GetComponent<IBullet>().Shoot(direction);
            /*
                This is not simple anymore, do this if you want to expand only -> NOW ONLY PREFABS WITH ALL
                Prefabs should have all relevant data already
                prefab get bullet script
                prefab add behaviour?
                prefab add animation?
            */
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
        if(mCurCore == null) mCurCore = DefaultCore;

        mFireRate *= mCurCore.BaseFireRateMod;
        mDmgPerShot *= mCurCore.BaseDmgMod;
    }

    private void AddBulletValue()
    {
        if(mCurBullet == null) mCurBullet = mCurCore.Bullets[0];

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
        return false;
    }

}