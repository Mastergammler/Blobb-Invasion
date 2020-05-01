using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Core;


namespace BlobbInvasion.Gameplay.Character.Enemies.mShieldEnemy
{
    [RequireComponent(typeof(IMoveable))]
    public class RobotEnemyMaster : MonoBehaviour, IObservable, IHighscoreEvent
    {
        public Transform PlayerPosition;
        public int AggressionRange;
        public float StoppingDistance;

        public event ScoreActionEvent ScoreEvent;

        //###############
        //##  MEMBERS  ##
        //###############

        private IMoveable mMoveHandler;
        private Callback mCallbacks;
        private Transform mShield;

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
            mShield = transform.GetChild(0);
        }

        private void Update()
        {
            checkPlayerDistance();
            mShield.position = Vector3.zero;
        }

        private void OnDestroy()
        {
            HighscoreManager.Instance.AddToScore(HighscoreManager.KILL_ENEMY);
            mCallbacks?.Invoke();
            ScoreEvent?.Invoke(ScoreType.KILLED_ENEMY);
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

            if (direction.x > 0 && transform.localScale.x > 0
                || direction.x < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }

            mMoveHandler.Move(direction);
        }
    }
}