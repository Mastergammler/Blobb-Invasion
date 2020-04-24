using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed;

    private Rigidbody2D mRigidBody;

    private const float SELF_DESTRUCT_TIME = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void initComponents()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    // Just adds the speed to the bullt
    public void Fly(Vector2 direction)
    {
        if(mRigidBody == null) initComponents();

        direction.Normalize();
        mRigidBody.velocity = new Vector2(direction.x * BulletSpeed, direction.y * BulletSpeed);
        StartCoroutine(initSelfDestructionSequence());
    }

    private IEnumerator initSelfDestructionSequence()
    {
        yield return new WaitForSeconds(SELF_DESTRUCT_TIME);
        Destroy(gameObject);
        yield return null;
    }
}
