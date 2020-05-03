using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    public class ReturnToPost : IState
    {
        private Vector3 mPostPosition;
        private Transform mOwnPosition;
        private IMoveable mMoveHandler;

        public ReturnToPost(Vector3 postPos,Transform ownPos,IMoveable moveable)
        {
            mPostPosition = postPos;
            mOwnPosition = ownPos;
            mMoveHandler = moveable;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            // nothing to do
        }

        public void Tick()
        {
            Vector2 direction = mPostPosition - mOwnPosition.position;
            mMoveHandler.Move(direction);
        }

        public void OnExit()
        {
            mMoveHandler.Move(Vector2.zero);
        }
    }
}