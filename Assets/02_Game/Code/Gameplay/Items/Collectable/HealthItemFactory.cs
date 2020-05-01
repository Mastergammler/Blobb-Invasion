using System;
using UnityEngine;

using BlobbInvasion.Core;

namespace BlobbInvasion.Gameplay.Items.Collectable
{
    //S: Factory for all kinds of collectables
    public class HealthItemFactory : EntityFactory<CollectableBase>
    {
        public HealthItemFactory(GameObject prefab, Highscore highscore):base(prefab,highscore){}
    }
}