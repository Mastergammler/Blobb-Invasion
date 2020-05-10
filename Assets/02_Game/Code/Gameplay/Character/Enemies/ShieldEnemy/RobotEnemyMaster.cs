using System.ComponentModel;
using System.Collections;
using System;
using UnityEngine;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Core;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States;
using BlobbInvasion.Gameplay.Character.Enemies.StateMachine;
using BlobbInvasion.Gameplay.Effects;
using BlobbInvasion.Code.Gameplay.Character;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    // S : Master controller for the robot enemie
    //      Manages and pulls all components it needs
    [RequireComponent(typeof(IMoveable))]
    public partial class RobotEnemyMaster : MonoBehaviour, IObservable, IHighscoreEvent
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
        private const float DISTANCE_TO_OBJ = 2f;
        private const float IDLE_ANIM_DELAY_TIME = 10f;
        private const String ANIMATOR_BOOL = "IsActive";
        private const float ATTACK_MOVE_SPEED_MULT = 1.5f;
        private const float MAX_ATTACK_TIME = 1f;

        //###############
        //##  MEMBERS  ##
        //###############

        private IMoveable mMoveHandler;
        private IColorChange mColorChanger;
        private IDetector mDetector;

        private Callback mCallbacks;
        private Transform mShield;
        private Vector3 mPostPosition;
        private StateMachine.StateMachine mStateMachineOld;
        private RobotStateMachine mStateMachine;
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

            // setting childs active to prevent collider ignored by physics bug
            //FIXME: still doesn't work
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

            mShield = transform.GetChild(0);
            mShield.GetComponent<Shield>().OnPlayerCollision += onPlayerCollision;
            mShield.GetComponent<Shield>().OnShieldDestroyed += () => mHasShield = false;
            mAlertCollider = GetComponentInChildren<CircleCollider2D>();
            mDetector = GetComponentInChildren<IDetector>();
            mDetector.SetTagsFilter(Tags.COLLECTABLE);
            mDetector.OnItemDetected += CollectableUpdate;
            mAttackState = new AttackPossible(this);
            mAnimator = GetComponent<Animator>();

            checkPlayerRef();
            initBehaviour();
            mStateMachine = new RobotStateMachine(this);
            mStateMachine.Initialize();
        }

        private void Update()
        {
            mTimeSinceLastUpdate += Time.deltaTime;
            mStateMachine.Tick();

            if (mTimeSinceLastUpdate > STATE_MACHINE_UPDATE_TIME)
            {
                mTimeSinceLastUpdate -= STATE_MACHINE_UPDATE_TIME;
                //fixme mStateMachine.EnableLogging(LogStateMachineChange);
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
            else if(other.tag.Equals(Tags.BULLET))
            {
                mAnimator.SetBool(ANIMATOR_BOOL,true);
            }
        }

        private event Action OnCollisionWithPlayer;
        private void onPlayerCollision()
        {
            mAttackState.ResetAttack();    
            StartCoroutine(stopPlayerNow());
        }

        private IEnumerator stopPlayerNow()
        {
            yield return new WaitForSeconds(0.05f);
            OnCollisionWithPlayer?.Invoke();
            yield return null;
        }

        private void CollectableUpdate(Collider2D collider)
        {
            mCurrentObjective = collider.transform;
            Debug.Log("Current objective set to : " + collider.transform.name);
            UpdateObjectiveInState(mCurrentObjective);
        }

        private void RunAsCoroutine(Action action, float waitTime)
        {
            StartCoroutine(GenericCoroutine(action, waitTime));
        }

        private IEnumerator GenericCoroutine(Action action, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            action();
            yield return null;
        }

        private void PlayIdleAnim(Action handler)
        {
            StartCoroutine(idleAnimWithDelay(handler));
        }

        private IEnumerator idleAnimWithDelay(Action handler)
        {
            yield return new WaitForSeconds(IDLE_ANIM_DELAY_TIME);
            mAnimator.SetBool(ANIMATOR_BOOL, false);
            handler?.Invoke();
            yield return null;
        }

        //-------------
        //    Debug
        //-------------

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), AggressionRange);
            UnityEditor.Handles.color = new Color(1, 0, 0, 0.2f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, new Vector3(0, 0, 1), AttackRange);

            CircleCollider2D cc = GetComponentInChildren<CircleCollider2D>();
            if(cc != null)
            {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), cc.radius);
            }
        }

        //###############
        //##  METHODS  ##
        //###############

        // FIXME: case for no objective found
        private Func<bool> isAlert() => () => distanceToPlayer() < mAlertCollider.radius;
        private Func<bool> notAlert() => () => distanceToPlayer() > mAlertCollider.radius;
        private Func<bool> atPost() => () => distanceToPost() <= StoppingDistance;
        private Func<bool> awayFromPost() => () => distanceToPost() > StoppingDistance;
        private Func<bool> isAggro() => () => isInAggroRange() & !isInStoppingRange();
        private Func<bool> canAttack() => () => distanceToPlayer() < AttackRange && mCanAttack && mHasShield;
        private Func<bool> idleAnim() => () => mAnimator.GetBool(ANIMATOR_BOOL);
        private Func<bool> stopOnPlayer() => () => distanceToPlayer() <= StoppingDistance;
        private Func<bool> noObjectiveAggro() => () => distanceToPlayer() < mAlertCollider.radius && mCurrentObjective == null;

        //------------------
        // Old Condititons
        //------------------

        private Func<bool> isNotChasing() => () => !isInAggroRange() || isInStoppingRange();
        private Func<bool> isAtCurrentObjective() => () => distanceToCurObj() <= DISTANCE_TO_OBJ;


        private void initBehaviour()
        {
            //mStateMachine = new StateMachine.StateMachine();

            // States
            var idleState = new Idle(mMoveHandler, mAnimator);
            var chasingState = new Chase(mMoveHandler, Player, transform);
            var returnState = new ReturnToPost(mPostPosition, transform, mMoveHandler);
            var bodySlam = new BodyAttack(mMoveHandler, mColorChanger, 1.5f, Player, transform);
            //fixme object null right here and doesn't get updated
            var protectionState = new ProtectObjective(Player, mMoveHandler);

            // Transitions
            /*AtPrio(bodySlam, canAttack());
            AtPrio(chasingState, isAggroChasing());
            At(returnState, idleState, notAtPost());
            At(idleState, chasingState, isNotChasing());
            //At(idleState, returnState, isAtPost());
            At(chasingState,idleState,isAggroChasing());
            At(chasingState,returnState,isAggroChasing());
            At(protectionState,idleState,inAlertRange());
            At(protectionState,returnState,inAlertRange());
            At(idleState,protectionState,isNotInAlertRange());*/


            //At(bodySlam,chasingState,canAttack());
            //At(bodySlam,idleState,canAttack());
            //At(chasingState,bodySlam,() => !mCanAttack && isInAggroRange());
            //At(idleState,bodySlam,() => !mCanAttack && isInStoppingRange());

            // Callbacks
            bodySlam.OnAttackStarted += () => mAttackState.ResetAttack();
            UpdateObjectiveInState = protectionState.ObjUpdateDel;

            // Initial state
            //mStateMachine.SetState(idleState);
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

        //void At(IState to, IState from, Func<bool> condition) => mStateMachine.AddTransition(from, to, condition);
        //void AtPrio(IState to, Func<bool> condition) => mStateMachine.AddAnyTransition(to, condition);
        private bool isInAggroRange() => distanceToPlayer() < AggressionRange;
        private bool isInStoppingRange() => distanceToPlayer() < StoppingDistance;
        private float distanceToPlayer() => Vector2.Distance(Player.position, transform.position);
        private float distanceToPost() => Vector2.Distance(mPostPosition, transform.position);
        private float distanceToCurObj() => mCurrentObjective != null ? Vector2.Distance(mCurrentObjective.position, transform.position) : 0;

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
            mCanAttack = false;
            yield return new WaitForSeconds(5f);
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