public delegate void Callback();
namespace BlobbInvasion. 
{
public interface
 IObservable
{
    void RegisterCallback(Callback cb);
}