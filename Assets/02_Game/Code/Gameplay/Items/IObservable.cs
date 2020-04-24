public delegate void Callback();
namespace BlobbInvasion.Gameplay.Items
{
    public interface IObservable
    {
        void RegisterCallback(Callback cb);
    }
}