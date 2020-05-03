using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: runs to the player to body slam him
    //      This will only be executed once per state change
    //      The position is not adjusted during the charge
    public class BodyAttack : IState
    {
        private IMoveable mMoveHandler;
        private float mSpeedMult;
        private bool mFirstAttack;

        private Transform mPlayerPos;
        private Transform mOwnPos;

        public BodyAttack(IMoveable moveable, float speedMultiplicator,Transform playerPos,Transform ownPos)
        {
            mMoveHandler = moveable;
            mSpeedMult = speedMultiplicator;
            mPlayerPos = playerPos;
            mOwnPos = ownPos;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            mFirstAttack = true;
        }

        public void Tick()
        {
            if (mFirstAttack)
            {
                Vector2 direction = mPlayerPos.position - mOwnPos.position;
                mMoveHandler.MoveFaster(direction, mSpeedMult);
                mFirstAttack = false;
            }
        }

        public void OnExit()
        {
            mMoveHandler.Move(Vector2.zero);
        }
    }
}