public delegate void Callback();
public interface IObservable
{
    void RegisterCallback(Callback cb);
}