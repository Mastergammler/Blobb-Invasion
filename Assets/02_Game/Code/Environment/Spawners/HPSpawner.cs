using System.Reflection.Emit;
using UnityEngine;
using System;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.Gameplay;

namespace BlobbInvasion.Environment.SpawnSystems
{
    public class HPSpawner : MonoBehaviour, IHpSpawner
    {
        public event EventHandler<SpawnerEventArgs> OnItemCollected;

        //###############
        //##  MEMBERS  ##
        //###############

        private IGOFactory mFactory;

        //################
        //##    MONO    ##
        //################

        private void Start()
        {
            mFactory = MasterFactory.Instance.GetFactory(FactoryType.ENEMY_SHIELD_ROBOT);
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void Spawn()
        {
            var gObj = mFactory.CreateGameObject(transform.position);

            CollectableBase cb = gObj.GetComponent<CollectableBase>();
            if (cb == null) Debug.LogWarning("Collectable does not extend from collectable base! No callback possible!");
            else cb.RequestCallback(HasBeenCollected);
        }

        public void HasBeenCollected()
        {
            OnItemCollected?.Invoke(this, new SpawnerEventArgs() { Position = transform });
        }

    }
}