using UnityEngine;

namespace BlobbInvasion.Gameplay.Effects
{
    //S: Changes the sprite color
    //      And changes back after a delay
    //      Or manually
    public interface IColorChange
    {
         bool AutoChangeBack { get; }
         void ChangeColor();
         void ChangeBack();
    }
}