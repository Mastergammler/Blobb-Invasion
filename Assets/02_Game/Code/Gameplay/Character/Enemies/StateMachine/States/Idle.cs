using UnityEngine;
using System;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: Implementation of the Idle state
    //      Default state, where the object is stopped
    public class Idle : IState
    {
        private IMoveable mMoveHandler;
        private Animator mAnimator;

        public const String ANIMATOR_PARAM = "IsActive";

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        public Idle(IMoveable movable, Animator animator)
        {
            mMoveHandler = movable;
            mAnimator = animator;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            mMoveHandler.Move(Vector2.zero);
            mAnimator.SetBool(ANIMATOR_PARAM,false);
        }

        public void Tick()
        {
            // do nothing
        }

        public void OnExit()
        {
            mAnimator.SetBool(ANIMATOR_PARAM,true);
        }

    }
}