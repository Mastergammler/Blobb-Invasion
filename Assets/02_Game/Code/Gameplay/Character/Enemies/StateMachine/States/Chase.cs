using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: Handle the enemy in chase state
    //      Chases the player until a stopping distance is reached or player is out of range again
    public class Chase : IState
    {
        private IMoveable mMoveHandler;
        private Transform mPlayerPos;
        private Transform mOwnPos;

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        public Chase(IMoveable moveable, Transform playerPos, Transform ownPos)
        {
            mMoveHandler = moveable;
            mPlayerPos = playerPos;
            mOwnPos = ownPos;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            // nothing to set up 
        }

        public void Tick()
        {
            moveTowardsPlayer();
        }

        private void moveTowardsPlayer()
        {
            // move towards appearently works the other way around
            Vector2 direction = mPlayerPos.position - mOwnPos.position;

            if (direction.x > 0 && mOwnPos.localScale.x > 0
                || direction.x < 0 && mOwnPos.localScale.x < 0)
            {
                mOwnPos.localScale = new Vector3(-mOwnPos.localScale.x, mOwnPos.localScale.y, 1);
            }

            mMoveHandler.Move(direction);
        }

        public void OnExit()
        {
            mMoveHandler.Move(Vector2.zero);
        }
    }
}