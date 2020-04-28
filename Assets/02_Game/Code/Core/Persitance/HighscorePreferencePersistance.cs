using System;
using UnityEngine;

namespace BlobbInavsion.Core.Persitance
{
    //S: Persists the score via the PlayerPreferences
    // Does not validate if the score is actually higher than before!
    //O: CLASS IS CLOSED
    public class HighscorePreferencePersistance : IHighscorePersitance
    {
        //#################
        //##  CONSTANTS  ##
        //#################

        private const String HIGHSCORE_KEY = "prefs-highscore-key";
        public HighscorePreferencePersistance(){}

        //###############################
        //##  I HIGHSCORE PERSISTANCE  ##
        //###############################

        public int getSavedHighscore()
        {
            return PlayerPrefs.GetInt(HIGHSCORE_KEY,0);
        }

        public void saveCurrentHighscore(int newScore)
        {
            PlayerPrefs.SetInt(HIGHSCORE_KEY,newScore);
        }
    }
}