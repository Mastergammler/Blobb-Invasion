using UnityEngine;
using System.Collections;

public class BulletRayBase : MonoBehaviour, IBullet 
{

    public float BulletSpeed { set; get;}
    private const float TIME_UNTIL_DESTROY = 0.05f;

    public bool DestroyImmediately = true;

     //###############
    //##  MEMBERS  ##
    //###############
    private LineRenderer mLine;

    //################
    //##    MONO    ##
    //################

    private void Awake()
    {
        mLine = GetComponent<LineRenderer>();
    }

    public void Shoot(Vector2 direction, float damage)
    {
        direction.Normalize();
        checkHitEnemy(direction,damage);
        StartCoroutine(DestroyAfterOneFrame());
    }


    private void checkHitEnemy(Vector2 direction, float damage)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,direction);

        mLine.SetPosition(0,transform.position);

        if(hit && hit.transform.tag.Equals(Tags.ENEMY))
        {
            IHealthManager enemyHp = hit.transform.GetComponent<IHealthManager>();
            enemyHp.LoseHealth(damage);

            float distance = Vector2.Distance(transform.position,hit.transform.position);
            Vector2 enemyPos = new Vector2(hit.transform.position.x,hit.transform.position.y);
            mLine.SetPosition(1,calculateEvenEnemyHitPoint(direction,enemyPos));
        }
        else
        {
            mLine.SetPosition(1,calculateEvenEndPoint(direction));
        }
    }

    private Vector2 calculateEvenEnemyHitPoint(Vector2 direction, Vector2 enemyPosition)
    {
       float dist =  Vector2.Distance(transform.position,enemyPosition);
       Vector2 dirVec = direction * dist;
       Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
       return curPos + dirVec;
    }

    private Vector2 calculateEvenEndPoint(Vector2 direction)
    {
        Vector2 directionVector = direction * 50;
        Vector2 shootDirection = new Vector2(transform.position.x,transform.position.y);
        return shootDirection + directionVector;
    }

    private IEnumerator DestroyAfterOneFrame()
    {
        if(DestroyImmediately)
        {
            yield return 1;
        }
        else
        {
            yield return new WaitForSeconds(TIME_UNTIL_DESTROY);
        }
        Destroy(gameObject);
        yield return null;
    }

}