using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMoveable))]
public class ShieldEnemy : MonoBehaviour, IObservable
{
    public Transform PlayerPosition;
    public int AggressionRange;
    public float StoppingDistance;
    
    //###############
    //##  MEMBERS  ##
    //###############

    private IMoveable mMoveHandler;
    private Callback mCallbacks;

    //################
    //##    MONO    ##
    //################

    private void Start() 
    {
        mMoveHandler = GetComponent<IMoveable>();
        if(PlayerPosition == null)
        {
            PlayerPosition = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        }
    }

    private void Update() 
    {
        checkPlayerDistance();
    }

    private void OnDestroy() 
    {
        mCallbacks?.Invoke();
    }

    //##################
    //##  OBSERVABLE  ##
    //##################

    public void RegisterCallback(Callback callback)
    {
        mCallbacks += callback;
    }

    //###############
    //##  METHODS  ##
    //###############



    private void checkPlayerDistance()
    {
        float distance = Vector2.Distance(PlayerPosition.position,transform.position);
        if(distance < AggressionRange && distance > StoppingDistance)
        {
            moveTowardsPlayer();
        }
        else
        {
            mMoveHandler.Move(new Vector2(0,0));
        }
    }

    private void moveTowardsPlayer()
    {
        // move towards appearently works the other way around
        Vector2 direction = PlayerPosition.position - transform.position;
        mMoveHandler.Move(direction);
    }
}
