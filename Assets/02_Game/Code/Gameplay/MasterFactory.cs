using System.Collections;
using BlobbInvasion.Core;
using UnityEngine;
using System;

using BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy;
using BlobbInvasion.Utilities.Exceptions;
using BlobbInvasion.Gameplay.Items.Collectable;
using BlobbInvasion.Gameplay.Items;

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
    // this stays a mono behaviour now, in order to assign the prefabs
    public sealed class MasterFactory : MonoBehaviour
    {
        //##################
        //##    EDITOR    ##
        //##################
        public GameObject HealthCollectablePrefab;
        public GameObject ShiledRobotEnemyPrefab;

        //###############
        //##  MEMBERS  ##
        //###############

        private Highscore rHighscore;
        private static MasterFactory sInstance;

        //#####################
        //##  INSTANTIATION  ##
        //#####################

        //private MasterFactory() { }
        private void Awake()
        {
            sInstance = this;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void SetHighscoreManager(Highscore highscoreRef)
        {
            rHighscore = highscoreRef;
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
            if (rHighscore == null)
                throw new ExecutionOrderException(
                    "GetFactory",
                    "SetHighscoreManager",
                    "Highscore events have to be assinged with the creation of instances.");
        }

        private IGOFactory createConcreteFactory(FactoryType type)
        {
            switch (type)
            {
                //todo
                case FactoryType.COLLECTABLE_HEALTH: return new HealthItemFactory(HealthCollectablePrefab, rHighscore);
                case FactoryType.ENEMY_SHIELD_ROBOT: return new RobotEnemyFactory(ShiledRobotEnemyPrefab, rHighscore);
                default: throw new MissingFieldException($"No handling defined for the tye: {type.ToString()}");
            }
        }

        //#################
        //##  ACCESSORS  ##
        //#################
        public static MasterFactory Instance => sInstance;
    }

    public enum FactoryType
    {
        COLLECTABLE_HEALTH,
        ENEMY_SHIELD_ROBOT
    }

}