using System;
using UnityEngine;

using BlobbInvasion.Core;

namespace BlobbInvasion.Gameplay.Items.Collectable
{
    public class HealthItemFactory : IGOFactory
    {

        //###############
        //##  MEMBERS  ##
        //###############
        private GameObject mPrefab;
        private Highscore rHighscore;

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        public HealthItemFactory(GameObject prefab, Type masterScript, Highscore highscore)
        {
            mPrefab = prefab;
            rHighscore = highscore;
            checkPrefabType(masterScript);
        }

        //##################
        //##  IGOFACTORY  ##
        //##################

        public GameObject CreateGameObject(Vector3 pos)
        {
            var obj = GameObject.Instantiate(mPrefab,pos,Quaternion.identity);
            obj.GetComponent<IHighscoreEvent>().ScoreEvent += rHighscore.ScoreActionInvoked;
            return obj;
        }

        public GameObject CreateGameObject(Vector3 pos,Quaternion rotation)
        {
            throw new NotImplementedException();
        }

        public GameObject CreateGameObject(Transform transform)
        {
            throw new NotImplementedException();
        }

        //#################
        //##  AUXILIARY  ##
        //#################

        private void checkPrefabType(Type expectedScriptType)
        {
            if(mPrefab.GetComponent(expectedScriptType) == null)
                    throw new MissingComponentException("Prefab and expected components do not match!");
            if(mPrefab.GetComponent<IHighscoreEvent>() == null)
                    throw new MissingComponentException("Prefab doesn't contain a type for the Highscore event!");
        }

    }
}