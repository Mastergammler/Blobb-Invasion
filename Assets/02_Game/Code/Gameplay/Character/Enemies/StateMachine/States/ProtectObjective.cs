using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine.States
{

    public delegate void ObjectiveChanged(Transform objective); 


    //S: handles the movement around a specific objective that is opposed to the player
    //      The position is calculated from the player and objective position
    public class ProtectObjective : IState
    {
        private const float DISTANCE_TO_OBJECTIVE = 2f;

        private Transform mPlayer;
        private Transform mObjective;
        private IMoveable mMovable;

        public ProtectObjective(Transform player, IMoveable movable)
        {
            mPlayer = player;
            mMovable = movable;
        }

        public void OnEnter()
        {
            // nothing specific
        }

        public void OnExit()
        {
            mMovable.Move(Vector2.zero);
        }

        public void Tick()
        {
            if(mObjective != null)
                mMovable.MoveTo(TargetPosition());
        }

        

        //#################
        //##  AUXILIARY  ##
        //#################

        private void SetObjective(Transform objective)
        {
            mObjective = objective;
        }

        private Vector2 TargetPosition()
        {
            Vector2 direction = mPlayer.position - mObjective.position;
            Vector2 objPos = mObjective.position;
            direction.Normalize();
            Vector2 movePos = direction * DISTANCE_TO_OBJECTIVE + objPos;
            //Debug.Log("Moving to: " + movePos);
            return movePos;
        }

        //#################
        //##  ACCESSORS  ##
        //#################

        public ObjectiveChanged ObjUpdateDel => SetObjective;
    }
}