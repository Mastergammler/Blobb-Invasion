using BlobbInvasion.Core;
using UnityEngine;
using System;

using BlobbInvasion.Utilities.Exceptions;

namespace BlobbInvasion.Gameplay
{
    //S: Factory that creates instances of a game object for you
    //The type describes the script of the object that is created
    //The factory is responsible of checking weather the prefab accords this constraints
    public interface IGOFactory
    {
        GameObject CreateGameObject(Vector3 position);
        GameObject CreateGameObject(Vector3 position, Quaternion rotation);
        GameObject CreateGameObject(Transform position);
    }

    //S: Manages and creates the concrete factories for the given type
    //O: IN DEVELOPMENT
    //L: NOT EXTENDABLE
    public sealed class MasterFactory
    {
        //###############
        //##  MEMBERS  ##
        //###############

        private Highscore mHighscoreRef;
        private static MasterFactory sInstance;

        //#####################
        //##  INSTANTIATION  ##
        //#####################
        private MasterFactory() { }

        //#################
        //##  INTERFACE  ##
        //#################

        public void SetHighscoreManager(Highscore highscoreRef)
        {
            mHighscoreRef = highscoreRef;
        }

        public IGOFactory GetFactory(FactoryType type)
        {
            validateRequirements();
            return createConcreteFactory(type);
        }

        //#################
        //##  AUXILIARY  ##
        //#################

        private void validateRequirements()
        {
            if (mHighscoreRef == null)
                throw new ExecutionOrderException(
                    "GetFactory",
                    "SetHighscoreManager",
                    "Highscore events have to be assinged with the creation of instances.");
        }

        private IGOFactory createConcreteFactory(FactoryType type)
        {
            switch(type)
            {
                //todo
                case FactoryType.COLLECTABLE_HEALTH: return null;
                case FactoryType.ENEMY_SHIELD_ROBOT: return null;
                default: throw new MissingFieldException($"No handling defined for the tye: {type.ToString()}");
            }
        }

        //#################
        //##  ACCESSORS  ##
        //#################
        public static MasterFactory Instance
        {
            get
            {
                if (sInstance == null) sInstance = new MasterFactory();
                return sInstance;
            }
        }
    }

    public enum FactoryType
    {
        COLLECTABLE_HEALTH,
        ENEMY_SHIELD_ROBOT
    }

}