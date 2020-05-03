using UnityEngine;

namespace BlobbInvasion.Gameplay.Character
{
    //S: Interface for objects that move around the map
    //      Handle movement of different kinds
    public interface IMoveable
    {
        //Moves into the direction until the object gets stopped (Vector2.zero)
        void Move(Vector2 direction);
        
        //Same as move just will increase the speed by the multiplicator
        void MoveFaster(Vector2 direction,float speedMultiplicator);
    }
}