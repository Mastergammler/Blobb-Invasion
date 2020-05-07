using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using System;
using BlobbInvasion.Gameplay.Effects;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: runs to the player to body slam him
    //      This will only be executed once per state change
    //      The position is not adjusted during the charge
    public class BodyAttack : IState
    {
        private bool mIsFirstTick;
        private float mSpeedMult;

        private Transform mPlayerPos;
        private Transform mOwnPos;
        private IMoveable mMoveHandler;
        private IColorChange mColorChanger;

        private const int MAX_ATTACK_TIME_MILLIS = 1000;

        public BodyAttack(IMoveable moveable, IColorChange colorChanger, float speedMultiplicator,Transform playerPos,Transform ownPos)
        {
            mMoveHandler = moveable;
            mSpeedMult = speedMultiplicator;
            mPlayerPos = playerPos;
            mOwnPos = ownPos;
            mColorChanger = colorChanger;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void OnEnter()
        {
            mIsFirstTick = true;
        }

        public void Tick()
        {
            if (mIsFirstTick)
            {
                mIsFirstTick = false;
                Vector2 direction = mPlayerPos.position - mOwnPos.position;
                mMoveHandler.MoveFaster(direction, mSpeedMult);
                mColorChanger.ChangeColor();

                OnAttackStarted?.Invoke();
                Task.Delay(MAX_ATTACK_TIME_MILLIS).ContinueWith(t => OnExit());
            }
        }



        public void OnExit()
        {
            mMoveHandler.Move(Vector2.zero);
            mColorChanger.ChangeBack();
        }

        //#################
        //##  ACCESSORS  ##
        //#################

        public event Action OnAttackStarted;
    }
}