using System;

public delegate void Callback();
namespace BlobbInvasion.Gameplay.Items
{
    public interface IObservable
    {
        event Action OnObservableAction;
    }
}