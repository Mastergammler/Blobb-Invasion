using UnityEngine;
using System;
using BlobbInvasion.Core;

namespace BlobbInvasion.Gameplay
{
    //S: Factory for instantiating robot objects
    //O: CLOSED-EXTENDABLE
    //L: Instantiate Prefab, Check prefab for component (ScriptType & Highscore), connect to highscore
    public abstract class EntityFactory<T> : IGOFactory
    {
        //###############
        //##  MEMBERS  ##
        //###############
        private GameObject mPrefab;
        private Highscore rHighscore;

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        public EntityFactory(GameObject prefab, Highscore highscore)
        {
            mPrefab = prefab;
            rHighscore = highscore;
            checkPrefabType();
        }

        //##################
        //##  IGOFACTORY  ##
        //##################

        public GameObject CreateGameObject(Vector3 pos)
        {
            var obj = GameObject.Instantiate(mPrefab, pos, Quaternion.identity);
            obj.GetComponent<IHighscoreEvent>().ScoreEvent += rHighscore.ScoreActionInvoked;
            return obj;
        }

        public GameObject CreateGameObject(Vector3 pos, Quaternion rotation)
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

        private void checkPrefabType()
        {
            if (mPrefab.GetComponent<T>() == null)
                throw new MissingComponentException("Prefab and expected components do not match!");
            if (mPrefab.GetComponent<IHighscoreEvent>() == null)
                throw new MissingComponentException("Prefab doesn't contain a type for the Highscore event!");
        }
    }
}