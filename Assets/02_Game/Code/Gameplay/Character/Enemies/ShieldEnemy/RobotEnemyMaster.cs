using System.Collections;
using System;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Core;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine;
using BlobbInvasion.Gameplay.Effects;

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
        [SerializeField]
        [Tooltip("Should be smaller than aggression range")]
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
        private IColorChange mColorChanger;

        private Callback mCallbacks;
        private Transform mShield;
        private Vector3 mPostPosition;
        private StateMachine.StateMachine mStateMachine;
        private float mTimeSinceLastUpdate = 0;

        private bool mCanAttack = true;
        private bool mHasShield = true;

        private IAttackResetState mAttackState;
        private Transform mCurrentObjective;
        private CircleCollider2D mAlertCollider;
        private Animator mAnimator;

        private ObjectiveChanged UpdateObjectiveInState;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mMoveHandler = GetComponent<IMoveable>();
            mColorChanger = GetComponent<IColorChange>();
            mPostPosition = new Vector3(transform.position.x, transform.position.y, 0);
            mShield = transform.GetChild(0);
            mShield.GetComponent<Shield>().OnPlayerCollision += onPlayerCollision;
            mShield.GetComponent<Shield>().OnShieldDestroyed += () => mHasShield = false;
            mAlertCollider = GetComponent<CircleCollider2D>();
            mAttackState = new AttackPossible(this);     
            mAnimator = GetComponent<Animator>();

            checkPlayerRef();
            initBehaviour();
        }

        private void Update()
        {
            mTimeSinceLastUpdate += Time.deltaTime;

            if (mTimeSinceLastUpdate > STATE_MACHINE_UPDATE_TIME)
            {
                mStateMachine.Tick();
                mTimeSinceLastUpdate -= STATE_MACHINE_UPDATE_TIME;
                mStateMachine.EnableLogging(LogStateMachineChange);
            }
        }

        private void OnDestroy()
        {
            OnObservableAction?.Invoke();
            ScoreEvent?.Invoke(ScoreType.KILLED_ENEMY);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals(Tags.PLAYER))
            {
                onPlayerCollision();
            }
        }

        private void onPlayerCollision()
        {
            mAttackState.ResetAttack();
            StartCoroutine(stopPlayerNow());
        }

        private IEnumerator stopPlayerNow()
        {
            yield return new WaitForSeconds(0.05f);
            mStateMachine.SetState(new Idle(mMoveHandler,mAnimator));
            yield return null;
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.tag.Equals(Tags.COLLECTABLE))
            {
                mCurrentObjective = other.transform;
                UpdateObjectiveInState(mCurrentObjective);
                //Debug.Log(transform.position + ": Found objective: " + other.transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position,new Vector3(0,0,1),AggressionRange);
            UnityEditor.Handles.color = new Color(1,0,0,0.2f);
            UnityEditor.Handles.DrawSolidDisc(transform.position,new Vector3(0,0,1),AttackRange);

            CircleCollider2D cc = GetComponent<CircleCollider2D>();
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position,new Vector3(0,0,1),cc.radius);
        }

        //###############
        //##  METHODS  ##
        //###############

        private void initBehaviour()
        {
            mStateMachine = new StateMachine.StateMachine();

            // States
            var idleState = new Idle(mMoveHandler,mAnimator);
            var chasingState = new Chase(mMoveHandler, Player, transform);
            var returnState = new ReturnToPost(mPostPosition, transform, mMoveHandler);
            var bodySlam = new BodyAttack(mMoveHandler, mColorChanger, 1.5f, Player, transform);
            //fixme object null right here and doesn't get updated
            var protectionState = new ProtectObjective(Player,mMoveHandler);

            // Conditions
            Func<bool> isAggroChasing() => () => isInAggroRange() & !isInStoppingRange();
            Func<bool> isNotChasing() => () => !isInAggroRange() || isInStoppingRange();
            Func<bool> notAtPost() => () => distanceToPost() > StoppingDistance & !isInAggroRange();
            Func<bool> isAtPost() => () => distanceToPost() <= StoppingDistance;
            Func<bool> canAttack() => () => distanceToPlayer() < AttackRange && mCanAttack && mHasShield;
            Func<bool> inAlertRange() => () => distanceToPlayer() < mAlertCollider.radius && mCurrentObjective != null;
            Func<bool> isNotInAlertRange() => () => distanceToPlayer() > mAlertCollider.radius;

            // Transitions
            AtPrio(bodySlam, canAttack());
            AtPrio(chasingState, isAggroChasing());
            At(returnState, idleState, notAtPost());
            At(idleState, chasingState, isNotChasing());
            At(idleState, returnState, isAtPost());
            At(chasingState,idleState,isAggroChasing());
            At(chasingState,returnState,isAggroChasing());
            At(protectionState,idleState,inAlertRange());
            At(protectionState,returnState,inAlertRange());
            At(idleState,protectionState,isNotInAlertRange());


            //At(bodySlam,chasingState,canAttack());
            //At(bodySlam,idleState,canAttack());
            //At(chasingState,bodySlam,() => !mCanAttack && isInAggroRange());
            //At(idleState,bodySlam,() => !mCanAttack && isInStoppingRange());

            // Callbacks
            bodySlam.OnAttackStarted += () => mAttackState.ResetAttack();
            UpdateObjectiveInState = protectionState.ObjUpdateDel;

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
        void AtPrio(IState to, Func<bool> condition) => mStateMachine.AddAnyTransition(to, condition);
        private bool isInAggroRange() => distanceToPlayer() < AggressionRange;
        private bool isInStoppingRange() => distanceToPlayer() < StoppingDistance;
        private float distanceToPlayer() => Vector2.Distance(Player.position, transform.position);
        private float distanceToPost() => Vector2.Distance(mPostPosition, transform.position);

        //##################
        //##    STATE     ##
        //##################

        interface IAttackResetState
        {
            void ResetAttack();
        }

        class AttackPossible : IAttackResetState
        {
            private RobotEnemyMaster mEnemy;

            public AttackPossible(RobotEnemyMaster enemy)
            {
                mEnemy = enemy;
                mEnemy.mCanAttack = true;
            }

            public void ResetAttack()
            {
                mEnemy.mAttackState = new ResetStartedState(mEnemy);
            }
        }

        class ResetStartedState : IAttackResetState
        {
            private RobotEnemyMaster mEnemy;
            public ResetStartedState(RobotEnemyMaster enemy)
            {
                mEnemy = enemy;
                mEnemy.StartCoroutine(mEnemy.resetAttack());
            }
            public void ResetAttack()
            {
                // attack already in process of resetting, nothing to do
            }
        }

        private IEnumerator resetAttack()
        {
            yield return new WaitForSeconds(1f);
            mCanAttack = false;
            yield return new WaitForSeconds(5f);
            Debug.Log("State is set to attack possible");
            mAttackState = new AttackPossible(this);
            yield return null;
        }




        //#################
        //##  ACCESSORS  ##
        //#################

        public event ScoreActionEvent ScoreEvent;
        public event Action OnObservableAction;
    }
}