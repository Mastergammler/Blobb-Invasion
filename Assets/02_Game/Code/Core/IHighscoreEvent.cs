namespace BlobbInvasion.Core
{

    // delegate for receiving score actions (such as, player collected something, enemy died etc)
    public delegate void ScoreActionEvent(ScoreType type);

    public interface IHighscoreEvent
    {
         event ScoreActionEvent ScoreEvent; 
    }
}