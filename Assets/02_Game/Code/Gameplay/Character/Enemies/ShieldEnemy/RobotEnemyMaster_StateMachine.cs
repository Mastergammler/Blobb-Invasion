using UnityEngine;
using System;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    //S:  Contains the state machine logic for the robot enemy master
    //      That it has access to all that the robot enemy needs
    public partial class RobotEnemyMaster
    {

        interface IRobotState
        {
            void Tick();
        }

        public const String ANIMATOR_BOOL = "IsActive";

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
                mCurentState = new IdleState(mParent,this);
            }

            public void Tick()
            {
                mCurentState.Tick();
            }


            #region States

            // how to change state then?

            private class IdleState : IRobotState
            {
                private RobotEnemyMaster mParent;
                private RobotStateMachine mStateMachine;
                public IdleState(RobotEnemyMaster parent, RobotStateMachine stateMachine)
                {
                    mParent = parent;
                    mStateMachine = stateMachine;
                    mParent.mMoveHandler.Move(Vector2.zero);
                    mParent.mAnimator.SetBool(ANIMATOR_BOOL, false);
                }

                public void Tick()
                {
                    //todo idle counter, and only then set to inactive
                    // nothing to do
                    checkForState(mParent.inAlertRange());
                }

                private void checkForState(Func<bool> condition)
                {
                    if(condition())
                    {
                        mStateMachine.mCurentState = new ProtectionState(mParent,mStateMachine);
                    }
                }
            }

            private class ProtectionState : IRobotState
            {
                private RobotEnemyMaster mParent;
                private RobotStateMachine mStateMachine;

                private const float DISTANCE_TO_OBJ = 2f;
                public ProtectionState(RobotEnemyMaster parent, RobotStateMachine stateMachine)
                {
                    mParent = parent;
                    mStateMachine = stateMachine;
                    mParent.mAnimator.SetBool(ANIMATOR_BOOL,true);
                }

                public void Tick()
                {
                    checkCondition(mParent.isNotInAlertRange());
                    if(mParent.mCurrentObjective != null)
                        mParent.mMoveHandler.MoveTo(TargetPosition());
                }

                private void checkCondition(Func<bool> condition)
                {
                    if(condition()) mStateMachine.mCurentState = new IdleState(mParent,mStateMachine);
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