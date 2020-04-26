using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Core;


namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
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
            if (PlayerPosition == null)
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
            HighscoreManager.Instance.AddToScore(HighscoreManager.KILL_ENEMY);
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
            float distance = Vector2.Distance(PlayerPosition.position, transform.position);
            if (distance < AggressionRange && distance > StoppingDistance)
            {
                moveTowardsPlayer();
            }
            else
            {
                mMoveHandler.Move(new Vector2(0, 0));
            }
        }

        private void moveTowardsPlayer()
        {
            // move towards appearently works the other way around
            Vector2 direction = PlayerPosition.position - transform.position;

            Transform shield = transform.GetChild(0);

            if(direction.x > 0) shield.localScale = new Vector3(-shield.localScale.x,shield.localScale.y,shield.localScale.y);
            else if(direction.x < 0) shield.localScale =  new Vector3(shield.localScale.x,shield.localScale.y,shield.localScale.y);

            mMoveHandler.Move(direction);
        }
    }
}