using UnityEngine;
using System;

using BlobbInavsion.Core.Persitance;

namespace BlobbInavsion.Core
{
    // S: Manages HOW the highscore is persited in the game
    public interface IHighscorePersitance
    {
        int getSavedHighscore();
        void saveCurrentHighscore(int highscore);
    }

    // delegate for receiving score actions (such as, player collected something, enemy died etc)
    public delegate void ScoreActionEvent(ScoreType type);

    // S: Responsibility is tracking the score in game, and the highscore
    // O: IN PROGRESS
    // L: Tracking current game score, saving highscore to disk, receiving highscore events
    public class Highscore
    {
        //#################
        //##  CONSTANTS  ##
        //#################

        public const int KILL_ENEMY = 100;
        public const int FIND_CORE = 1000;
        public const int FOUND_WEAPON = 1000;
        public const int FIND_BULLET = 500;
        public const int GET_HEALTH = 50;

        private const String HIGHSCORE_KEY = "prefs-highscore-key";

        //###############
        //##  MEMBERS  ##
        //###############

        private IHighscorePersitance mPersitance;

        private int mCurrentScore;
        private int mHighscore;

        //###################
        //##  CONSTRUCTOR  ##
        //###################

        public Highscore() 
        { 
            mPersitance = new HighscorePreferencePersistance();
        }

        //###############
        //##  METHODS  ##
        //###############

        public void ScoreActionInvoked(ScoreType type)
        {
            switch(type)
            {
                case ScoreType.COLLECTED_WEAPON: mCurrentScore += FOUND_WEAPON; break;
                case ScoreType.COLLECTED_HEALTH: mCurrentScore += GET_HEALTH; break;
                case ScoreType.COLLECTED_BULLET: mCurrentScore += FIND_BULLET; break;
                case ScoreType.FOUND_CORE: mCurrentScore += FIND_CORE; break;
                case ScoreType.KILLED_ENEMY: mCurrentScore += KILL_ENEMY; break;
                default: throw new MissingFieldException($"Handling for score type {type.ToString()} not defined");
            }
        }
    }

    public enum ScoreType
    {
        FOUND_CORE, COLLECTED_HEALTH, COLLECTED_WEAPON,
        KILLED_ENEMY, COLLECTED_BULLET
    }
}