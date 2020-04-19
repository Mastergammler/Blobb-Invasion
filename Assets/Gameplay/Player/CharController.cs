using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class CharController : MonoBehaviour
{
    private static int HP_GAIN_VALUE = 20;
    private const float STUN_TIME = .5f;
    public float MovementSpeed = 0.1f;
    public GameObject bullet;

    //***************
    //**  MEMBERS  **
    //***************

    private SpriteRenderer mSpriteRenderer;
    private Vector2 mCurMovement;
    private Vector2 mDefaultDirection;
    private Transform mGunHinge;
    private bool isStunned = false;
    private bool isShooting = false;
    private bool lastShootingState = false;
    private AudioSource mAudioSource;

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
        mAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(mCurMovement.x >0) transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        else if(mCurMovement.x < 0) transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);

        mGunHinge.rotation = Quaternion.LookRotation(Vector3.forward,new Vector3(mCurMovement.x,mCurMovement.y,0));

        if(isShooting) justShoot();
    }
        
    //*****************
    //**  Interface  **
    //*****************

    public void Shoot(InputAction.CallbackContext context)
    {
        //if(bullet == null) Debug.Log("Bullet not set!");

        //Transform spawn = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).transform;
        //GameObject newBullet = Instantiate(bullet,spawn.position,spawn.rotation);

        context.action.started += (ctx) => { isShooting = true; lastShootingState = false; };
        context.action.canceled += (ctx) => { isShooting = false; lastShootingState = true; };

        //newBullet.GetComponent<BulletScript>().Fly(bulletDirection);
        //GetComponent<ISpriteMaterialChanger>().ChangeMaterial();
    }


    private void justShoot()
    {
        Vector2 bulletDirection = mCurMovement;
        if(mCurMovement == Vector2.zero) bulletDirection = mDefaultDirection;
        mWeaponHandler.Shoot(bulletDirection);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(isStunned) 
        {
            mMovementHandler.Move(Vector2.zero);
            return;
        }

        Vector2 newMovement = context.action.ReadValue<Vector2>();

        if(newMovement == Vector2.zero)
        {
            // this makes no sense
            //mCachedDirection = mCurMovement;
        }

        mCurMovement = newMovement;
        mMovementHandler.Move(newMovement);
    }



    private IEnumerator resetStun()
    {
        yield return new WaitForSeconds(STUN_TIME);
        isStunned = false;
        yield return null;
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag.Equals(Tags.ENEMY))
        {
            mAudioSource.Stop();
            mAudioSource.Play();
            isStunned = true;
            StartCoroutine(resetStun());
            return;
        }

        ICollectable collectable = other.GetComponent<ICollectable>();
        // we only care if we collid with an collectable
        if(collectable != null)
        {
            CollectableType type = collectable.Collect();
            if(type == CollectableType.NONE) return;

            if(type == CollectableType.HP_BOBBLE)
            {
                HighscoreManager.Instance.AddToScore(HighscoreManager.GET_HEALTH);
                mHealthManager.GainHealth(HP_GAIN_VALUE);
                return;
            }

            HighscoreManager.Instance.AddToScore(HighscoreManager.FIND_CORE);
            mInventory.AddInventoryItem(type);
        }
    }
}
