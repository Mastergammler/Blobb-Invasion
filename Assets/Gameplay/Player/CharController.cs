using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CharController : MonoBehaviour
{
    private static int HP_GAIN_VALUE = 20;
    public float MovementSpeed = 0.1f;
    public GameObject bullet;

    //***************
    //**  MEMBERS  **
    //***************

    private SpriteRenderer mSpriteRenderer;
    private Vector2 mCurMovement;
    private Vector2 mDefaultDirection;
    private Transform mGunHinge;

    //-------------------
    //  Player Systems
    //-------------------

    private IMoveable mMovementHandler;
    private IHealthManager mHealthManager;
    private IInventory mInventory;
    private IWeaponSystem mWeaponHandler;


    //************
    //**  MONO  **
    //************

    void Start()
    {
        mDefaultDirection = new Vector2(0,1);
        mGunHinge = transform.GetChild(0);
        mMovementHandler = GetComponent<IMoveable>();
        mHealthManager = GetComponent<IHealthManager>();
        mInventory = GetComponent<IInventory>();
        mWeaponHandler = GetComponent<IWeaponSystem>();
    }

    void Update()
    {
        if(mCurMovement.x == 1) transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        else if(mCurMovement.x == -1) transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);

        mGunHinge.rotation = Quaternion.LookRotation(Vector3.forward,new Vector3(mCurMovement.x,mCurMovement.y,0));
    }
        
    //*****************
    //**  Interface  **
    //*****************

    public void Shoot(InputAction.CallbackContext context)
    {
        //if(bullet == null) Debug.Log("Bullet not set!");

        //Transform spawn = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).transform;
        //GameObject newBullet = Instantiate(bullet,spawn.position,spawn.rotation);

        Vector2 bulletDirection = mCurMovement;
        if(mCurMovement == Vector2.zero) bulletDirection = mDefaultDirection;
        mWeaponHandler.Shoot(bulletDirection);
        //newBullet.GetComponent<BulletScript>().Fly(bulletDirection);
        //GetComponent<ISpriteMaterialChanger>().ChangeMaterial();
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 newMovement = context.action.ReadValue<Vector2>();

        if(newMovement == Vector2.zero)
        {
            // this makes no sense
            //mCachedDirection = mCurMovement;
        }

        mCurMovement = newMovement;
        mMovementHandler.Move(newMovement);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        
        // we only care if we collid with an collectable
        if(collectable != null)
        {
            CollectableType type = collectable.Collect();

            if(type == CollectableType.HP_BOBBLE)
            {
                mHealthManager.GainHealth(HP_GAIN_VALUE);
                return;
            }

            mInventory.AddInventoryItem(type);
        }
    }
}
