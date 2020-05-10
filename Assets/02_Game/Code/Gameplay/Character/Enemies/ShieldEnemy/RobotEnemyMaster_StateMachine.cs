using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    //S:  Contains the state machine logic for the robot enemy master
    //      That it has access to all that the robot enemy needs
    public partial class RobotEnemyMaster
    {

        //###############
        //##  ACTIONS  ##
        //###############

        #region Actions

        public delegate void RobotActionDefinition(RobotAction action, Func<bool> condition);
        public delegate void RobotAction(Action cb);

        interface IRobotAction
        {
            event Action OnActionFinished;
            Func<bool> Condition { get; }
            void Execute();
        }

        class DefaultRobotAction : IRobotAction
        {
            private RobotAction mAction;
            public DefaultRobotAction(RobotAction action, Func<bool> condition)
            {
                mAction = action;
                Condition = condition;
            }

            // --  Interface  --
            public void Execute()
            {
                mAction(OnActionFinished);
            }

            // -- Accessors --
            public Func<bool> Condition { private set; get; }
            public event Action OnActionFinished;
        }

        class ShowIdleAnim : IRobotAction
        {
            private RobotAction mAction;
            public ShowIdleAnim(RobotAction action, Func<bool> condition)
            {
                mAction = action;
                Condition = condition;
            }

            public Func<bool> Condition { private set; get; }

            public event Action OnActionFinished;
            public void Execute()
            {
                mAction(OnActionFinished);
            }
        }

        class EnemyAttack : IRobotAction
        {

            private RobotAction mAction;

            public EnemyAttack(RobotAction action, Func<bool> condition)
            {
                mAction = action;
                Condition = condition;
            }
            public void Execute()
            {
                mAction(OnActionFinished);
            }
            public Func<bool> Condition { private set; get; }
            public event Action OnActionFinished;
        }

        #endregion

        //##############
        //##  STATES  ##
        //##############

        interface IRobotState
        {
            void Tick();
        }

        class RobotStateMachine
        {
            private IRobotState mCurentState;
            private RobotEnemyMaster mParent;

            public RobotStateMachine(RobotEnemyMaster parent)
            {
                mParent = parent;
            }

            public void Initialize()
            {
                mCurentState = new IdleState(mParent, this);
            }

            public void Tick()
            {
                mCurentState.Tick();
            }


            #region States

            abstract class RobotState : IRobotState
            {
                public abstract void Tick();

                protected bool checkIf(Func<bool> condition)
                {
                    return condition();
                }

                protected bool checkIf(params Func<bool>[] conditions)
                {
                    bool result = true;

                    foreach (Func<bool> c in conditions)
                    {
                        result = result && c();
                    }

                    return result;
                }
            }


            // how to change state then?

            private class IdleState : RobotState
            {
                private RobotEnemyMaster mParent;
                private RobotStateMachine mStateMachine;
                private IRobotAction mAction;
                private bool mBlockedByAction = false;
                private Coroutine mCurCoroutine;
                public IdleState(RobotEnemyMaster parent, RobotStateMachine stateMachine)
                {
                    mParent = parent;
                    mStateMachine = stateMachine;
                    mParent.mMoveHandler.Move(Vector2.zero);
                    mAction = new DefaultRobotAction(resetIdleAnimAction, mParent.idleAnim());
                    mAction.OnActionFinished += () => mBlockedByAction = false;
                    Debug.Log("Idle State");
                }

                private void resetIdleAnimAction(Action cb)
                {
                    mBlockedByAction = true;
                    mCurCoroutine = mParent.StartCoroutine(idleAnimWithDelay(cb));
                }

                private IEnumerator idleAnimWithDelay(Action handler)
                {
                    yield return new WaitForSeconds(IDLE_ANIM_DELAY_TIME);
                    mParent.mAnimator.SetBool(ANIMATOR_BOOL, false);
                    handler?.Invoke();
                    yield return null;
                }

                public override void Tick()
                {
                    if (checkIf(mParent.isAlert()))
                    {
                        if (mCurCoroutine != null)
                        {
                            mParent.StopCoroutine(mCurCoroutine);
                            mCurCoroutine = null;
                        }
                        mStateMachine.mCurentState = new ProtectionState(mParent, mStateMachine);
                    }
                    else if (checkIf(mAction.Condition) & !mBlockedByAction)
                        mAction.Execute();
                    else
                        mParent.mMoveHandler.Move(Vector2.zero);
                }
            }

            //-------------
            //  Chasing
            //-------------

            private class ChasingState : RobotState
            {
                private RobotEnemyMaster mParent;
                private RobotStateMachine mStateMachine;
                private IRobotAction mRobotAction;
                private Task mCurrentTask;
                private bool mBlockedByAction = false;
                private CancellationTokenSource mSource;
                private Coroutine mCurCoroutine;

                public ChasingState(RobotEnemyMaster parent, RobotStateMachine stateMachine)
                {
                    mParent = parent;
                    mStateMachine = stateMachine;
                    mRobotAction = new DefaultRobotAction(enemyAttackAction, mParent.canAttack());
                    mParent.OnCollisionWithPlayer += StopAttackNow;
                    mRobotAction.OnActionFinished += () => mBlockedByAction = false;
                    Debug.Log("Chasing State");
                }

                private void StopAttackNow()
                {
                    if (mCurCoroutine == null) return;
                    mParent.StopCoroutine(mCurCoroutine);
                    mCurCoroutine = null;
                    mParent.mColorChanger.ChangeBack();
                    mParent.mMoveHandler.Move(Vector2.zero);
                    mBlockedByAction = false;
                }

                public override void Tick()
                {
                    if (checkIf(mParent.notAlert()))
                        mStateMachine.mCurentState = new ProtectionState(mParent, mStateMachine);
                    else if (checkIf(mRobotAction.Condition) & !mBlockedByAction)
                        mRobotAction.Execute();
                    else if (!mBlockedByAction)
                        moveTowardsPlayer();
                }

                private void enemyAttackAction(Action cb)
                {
                    mBlockedByAction = true;

                    Vector2 direction = mParent.Player.position - mParent.transform.position;
                    mParent.mMoveHandler.MoveFaster(direction, ATTACK_MOVE_SPEED_MULT);
                    mParent.mColorChanger.ChangeColor();
                    mParent.mAttackState.ResetAttack();

                    mCurCoroutine = mParent.StartCoroutine(AttackFinisnhed(cb));
                }

                private IEnumerator AttackFinisnhed(Action cb)
                {
                    yield return new WaitForSeconds(MAX_ATTACK_TIME);

                    mParent.mColorChanger.ChangeBack();
                    mParent.mMoveHandler.Move(Vector2.zero);
                    cb.Invoke();

                }

                private void moveTowardsPlayer()
                {
                    if (mBlockedByAction) return;

                    // move towards appearently works the other way around
                    Vector2 direction = mParent.Player.position - mParent.transform.position;

                    if (direction.x > 0 && mParent.transform.localScale.x > 0
                        || direction.x < 0 && mParent.transform.localScale.x < 0)
                    {
                        mParent.transform.localScale = new Vector3(-mParent.transform.localScale.x, mParent.transform.localScale.y, 1);
                    }

                    mParent.mMoveHandler.Move(direction);
                }

            }

            private class ProtectionState : RobotState
            {
                private RobotEnemyMaster mParent;
                private RobotStateMachine mStateMachine;

                public ProtectionState(RobotEnemyMaster parent, RobotStateMachine stateMachine)
                {
                    mParent = parent;
                    mStateMachine = stateMachine;
                    mParent.mAnimator.SetBool(ANIMATOR_BOOL, true);
                    Debug.Log("Protection State");
                }

                public override void Tick()
                {
                    //fixme idle condition has to be different
                    checkCondition(new Func<bool>[]
                        { mParent.isAtCurrentObjective(),mParent.notAlert()}
                        , mParent.isAggro());

                    if (mParent.mCurrentObjective != null)
                        mParent.mMoveHandler.MoveTo(TargetPosition());
                }

                private void checkCondition(Func<bool>[] idleConditions, Func<bool> chaseCondition)
                {

                    if (idleConditions[0]() && idleConditions[1]())
                    {
                        Debug.Log($"Going idle: Distance to obj: {mParent.distanceToCurObj()}");
                        mStateMachine.mCurentState = new IdleState(mParent, mStateMachine);
                    }
                    if (chaseCondition()) mStateMachine.mCurentState = new ChasingState(mParent, mStateMachine);
                }

                private Vector2 TargetPosition()
                {
                    Vector2 direction = mParent.Player.position - mParent.mCurrentObjective.position;
                    Vector2 objPos = mParent.mCurrentObjective.position;
                    direction.Normalize();
                    Vector2 movePos = direction * DISTANCE_TO_OBJ + objPos;
                    return movePos;
                }
            }
        }

        #endregion

    }
}