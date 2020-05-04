using UnityEngine;
using BlobbInvasion.Gameplay.Effects;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{
    //S: runs to the player to body slam him
    //      This will only be executed once per state change
    //      The position is not adjusted during the charge
    public class BodyAttack : IState
    {
        private bool mFirstAttack;
        private float mSpeedMult;

        private Transform mPlayerPos;
        private Transform mOwnPos;
        private IMoveable mMoveHandler;
        private IColorChange mColorChanger;

        private event AttackExecutedCallback mAttackEvent;

        public delegate void AttackExecutedCallback();
        public BodyAttack(IMoveable moveable, IColorChange colorChanger,
                float speedMultiplicator,Transform playerPos,Transform ownPos,
                AttackExecutedCallback cb)
        {
            mMoveHandler = moveable;
            mSpeedMult = speedMultiplicator;
            mPlayerPos = playerPos;
            mOwnPos = ownPos;
            mColorChanger = colorChanger;
            mAttackEvent += cb;
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
                mColorChanger.ChangeColor();
                mAttackEvent?.Invoke();
            }
        }

        public void OnExit()
        {
            mMoveHandler.Move(Vector2.zero);
            mColorChanger.ChangeBack();
        }
    }
}