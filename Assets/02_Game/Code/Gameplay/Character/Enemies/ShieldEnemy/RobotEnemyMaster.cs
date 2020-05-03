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

        [SerializeField]
        private Transform Player;
        [SerializeField]
        private int AggressionRange;
        [SerializeField]
        private float StoppingDistance;

        //###############
        //##  MEMBERS  ##
        //###############

        private IMoveable mMoveHandler;
        private Callback mCallbacks;
        private Transform mShield;
        private StateMachine.StateMachine mStateMachine;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMoveHandler = GetComponent<IMoveable>();
            mShield = transform.GetChild(0);

            checkPlayerRef();
            initStates();
        }

        private void Update()
        {
            mStateMachine.Tick();
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

            // Init States
            var idleState = new Idle(mMoveHandler);
            var chasingState = new Chase(mMoveHandler, Player, transform);

            // Init Conditions
            Func<bool> isAggro() => () => isInAggroRange()! & isInStoppingRange();
            Func<bool> isNotAggro() => () => !isInAggroRange() || isInStoppingRange();

            // Init Transitions
            At(idleState, chasingState, isAggro());
            At(chasingState, idleState, isNotAggro());

            // Set Init state
            mStateMachine.SetState(idleState);
        }

        private void checkPlayerRef()
        {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
            }
        }

        //#################
        //##  AUXILIARY  ##
        //#################

        void At(IState from, IState to, Func<bool> condition) => mStateMachine.AddTransition(from, to, condition);
        private bool isInAggroRange() => distanceToPlayer() < AggressionRange;
        private bool isInStoppingRange() => distanceToPlayer() < StoppingDistance;
        private float distanceToPlayer() => Vector2.Distance(Player.position, transform.position);

        //#################
        //##  ACCESSORS  ##
        //#################

        public event ScoreActionEvent ScoreEvent;
    }
}