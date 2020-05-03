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
        [SerializeField][Tooltip("Should be smaller than aggression range")]
        private float AttackRange;
        [SerializeField]
        private float AttackRate;
        [SerializeField]
        private bool LogStateMachineChange = false;

        //#################
        //##  CONSTANTS  ##
        //#################

        private const float STATE_MACHINE_UPDATE_TIME = 0.2f;

        //###############
        //##  MEMBERS  ##
        //###############

        private IMoveable mMoveHandler;
        private Callback mCallbacks;
        private Transform mShield;
        private Vector3 mPostPosition;
        private StateMachine.StateMachine mStateMachine;
        private float mTimeSinceLastUpdate = 0;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMoveHandler = GetComponent<IMoveable>();
            mShield = transform.GetChild(0);
            mPostPosition = new Vector3(transform.position.x,transform.position.y,0);

            checkPlayerRef();
            initBehaviour();
        }

        private void Update()
        {
            mTimeSinceLastUpdate += Time.deltaTime;

            if(mTimeSinceLastUpdate > STATE_MACHINE_UPDATE_TIME)
            {
                mStateMachine.Tick();
                mTimeSinceLastUpdate -= STATE_MACHINE_UPDATE_TIME;
                mStateMachine.EnableLogging(LogStateMachineChange);
            }
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

        private void initBehaviour()
        {
            mStateMachine = new StateMachine.StateMachine();

            // States
            var idleState = new Idle(mMoveHandler);
            var chasingState = new Chase(mMoveHandler, Player, transform);
            var returnState = new ReturnToPost(mPostPosition,transform,mMoveHandler);
            var bodySlam = new BodyAttack(mMoveHandler,1.5f,Player,transform);

            // Conditions
            Func<bool> isAggroChasing() => () => isInAggroRange() &! isInStoppingRange();
            Func<bool> isNotChasing() => () => !isInAggroRange() || isInStoppingRange();
            Func<bool> notAtPost() => () => distanceToPost() > StoppingDistance &! isInAggroRange();
            Func<bool> isAtPost() => () => distanceToPost() <= StoppingDistance;
            Func<bool> inAttackRange() => () => distanceToPlayer() < AttackRange;

            // Transitions
            AtPrio(bodySlam,inAttackRange());
            AtPrio(chasingState, isAggroChasing());
            At(returnState,idleState, notAtPost());
            At(idleState, chasingState, isNotChasing());
            At(idleState,returnState,isAtPost());

            // Initial state
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

        void At(IState to, IState from, Func<bool> condition) => mStateMachine.AddTransition(from, to, condition);
        void AtPrio(IState to, Func<bool> condition) => mStateMachine.AddAnyTransition(to,condition);
        private bool isInAggroRange() => distanceToPlayer() < AggressionRange;
        private bool isInStoppingRange() => distanceToPlayer() < StoppingDistance;
        private float distanceToPlayer() => Vector2.Distance(Player.position, transform.position);
        private float distanceToPost() => Vector2.Distance(mPostPosition,transform.position);

        //#################
        //##  ACCESSORS  ##
        //#################

        public event ScoreActionEvent ScoreEvent;
    }
}