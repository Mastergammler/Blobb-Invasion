namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine
{
    //Definition for a state
    public interface IState
    {
        //Is called every update (or other kind of tick)
         void Tick();
         //Is called when this state is initialized
         void OnEnter();
         //Is called before a new state is initialized
         void OnExit();
    }
}