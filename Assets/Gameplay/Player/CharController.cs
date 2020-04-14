using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CharController : MonoBehaviour
{

    private static int HP_GAIN_VALUE = 20;

    [SerializeField]
    private float HP = 100;
    private float mMaxHp;
    [SerializeField]
    private float HealthLossPerTick = 0.01f;


    private Color mStarHUE;
    [SerializeField]
    private Color mTargetHUE;

    public float MovementSpeed = 0.1f;
    public GameObject bullet;

    //***************
    //**  MEMBERS  **
    //***************

    private SpriteRenderer mSpriteRenderer;
    private Vector2 mCurMovement;
    private Vector2 mCachedDirection;

    private Transform mGunHinge;



    private IMoveable mMovementHandler;


    // Preemptive inventory
    private Dictionary<CollectableType,int> mInventory = new Dictionary<CollectableType, int>();




    //************
    //**  MONO  **
    //************

    void Start()
    {
        mCachedDirection = new Vector2(1,0);
        mGunHinge = transform.GetChild(0);
        mMovementHandler = GetComponent<IMoveable>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mStarHUE = mSpriteRenderer.color;
        mMaxHp = HP;
    }

    void Update()
    {
        if(mCurMovement.x == 1) transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        else if(mCurMovement.x == -1) transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);

        mGunHinge.rotation = Quaternion.LookRotation(Vector3.forward,new Vector3(mCurMovement.x,mCurMovement.y,0));

        loseHealth();
    }


    private void loseHealth()
    {
        HP -= HealthLossPerTick * Time.deltaTime;
        adjustColor();

        if(HP <= 0) Die();
    }

    private void adjustColor()
    {
        float hpPercentage = HP/mMaxHp;
        Color adjustedColor = Color.Lerp(mTargetHUE,mStarHUE,hpPercentage);
        adjustedColor.a = 1;
        mSpriteRenderer.color = adjustedColor;
    }

    private void gainHp(int hp)
    {
        if(HP + hp > mMaxHp)
        {
            HP = mMaxHp;
        }
        else
        {
            HP += hp;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        gameObject.SetActive(false);
    }
        
    //*****************
    //**  Interface  **
    //*****************

    public void Shoot(InputAction.CallbackContext context)
    {
        if(bullet == null) Debug.Log("Bullet not set!");

        Transform spawn = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).transform;
        GameObject newBullet = Instantiate(bullet,spawn.position,spawn.rotation);

        Vector2 bulletDirection = mCurMovement;
        if(mCurMovement == Vector2.zero) bulletDirection = mCachedDirection;

        newBullet.GetComponent<BulletScript>().Fly(bulletDirection);
        GetComponent<ISpriteMaterialChanger>().ChangeMaterial();
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 newMovement = context.action.ReadValue<Vector2>();

        if(newMovement == Vector2.zero)
        {
            mCachedDirection = mCurMovement;
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
                gainHp(HP_GAIN_VALUE);
                return;
            }


            int curAmount = 1;

            if(mInventory.ContainsKey(type))
            {
                mInventory.TryGetValue(type,out curAmount);
                mInventory.Remove(type);
                curAmount++;
            }

            mInventory.Add(type,curAmount);


            int value = -1;
            mInventory.TryGetValue(type,out value);
            Debug.Log("Current value of: " + type.ToString() + " is " + value);
        }
    }
}
