using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: Implementation of the Idle state
    //      Default state, where the object is stopped
    public class Idle : IState
    {
        private IMoveable mMoveHandler;

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        public Idle(IMoveable movable)
        {
            mMoveHandler = movable;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            mMoveHandler.Move(Vector2.zero);
        }

        public void Tick()
        {
            // do nothing
        }

        public void OnExit()
        {
            // do nothing
        }

    }
}