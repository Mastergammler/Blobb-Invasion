using UnityEngine;
using BlobbInvasion.Core;
using System;

namespace BlobbInvasion.Gameplay.Character.Enemies.ShieldEnemy
{
    //S: Factory for instantiating robot objects
    public class RobotEnemyFactory : EntityFactory<RobotEnemyMaster>
    {
        public RobotEnemyFactory(GameObject prefab, Highscore highscore) : base(prefab,highscore) {}
    }
}