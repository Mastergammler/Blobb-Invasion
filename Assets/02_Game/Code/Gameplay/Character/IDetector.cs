using UnityEngine;

namespace BlobbInvasion.Code.Gameplay.Character
{

    //Delegate for the detection event 
    public delegate void ItemDetected(Collider2D itemCollision);

    //S: Dectects items via a collider
    //      Then fires an event to notify who ever is interested
    public interface IDetector
    {
        event ItemDetected OnItemDetected;

        // sets the tags to filter for
        // if no tag is specified it will be invoked for every colliding element
        void SetTagsFilter(params string[] tags);
    }
}