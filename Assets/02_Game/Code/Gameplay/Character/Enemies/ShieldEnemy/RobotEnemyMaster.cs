using System;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Core;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    // S : Master controller for the robot enemie
    //      Manages and pulls all components it needs
    [RequireComponent(typeof(IMoveable))]
    public class RobotEnemyMaster : MonoBehaviour, IObservable, IHighscoreEvent
    {

        //##################
        //##    EDITOR    ##
        //##################

        public Transform PlayerPosition;
        public int AggressionRange;
        public float StoppingDistance;


        //###############
        //##  MEMBERS  ##
        //###############

        private IMoveable mMoveHandler;
        private Callback mCallbacks;
        private Transform mShield;
        private StateMachine.StateMachine mStateMachine;

        public event ScoreActionEvent ScoreEvent;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMoveHandler = GetComponent<IMoveable>();
            mShield = transform.GetChild(0);

            if (PlayerPosition == null)
            {
                PlayerPosition = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
            }
            initStates();
        }

        private void Update()
        {
            mStateMachine.Tick();
            //fixme is that corret? mShield.position = Vector3.zero;
        }

        private void OnDestroy()
        {
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

        private void initStates()
        {
            mStateMachine = new StateMachine.StateMachine();

            var idleState = new Idle(mMoveHandler);
            var chasingState = new Chase(mMoveHandler,PlayerPosition,transform);

            At(idleState,chasingState,isAggro());
            At(chasingState,idleState,isNotAggro());

            Func<bool> isAggro() => () => dist() < AggressionRange && dist() > StoppingDistance;
            Func<bool> isNotAggro() => () => dist() > AggressionRange || dist() < StoppingDistance;

            void At(IState from, IState to, Func<bool> condition) => mStateMachine.AddTransition(from,to,condition);
            float dist() => Vector2.Distance(PlayerPosition.position,transform.position);

            mStateMachine.SetState(idleState);
        }

    }
}