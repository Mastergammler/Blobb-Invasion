using UnityEngine;
using BlobbInvasion.Gameplay.Items.Crafting;

// Object for creating or holding the scriptable objects
namespace BlobbInvasion.Gameplay.Items
{
    public class CollectableFactory : MonoBehaviour
    {

        //*****************
        //**  INSPECTOR  **
        //*****************

        public BulletData BigBullet;
        public BulletData ExplodingBullet;
        public WeaponData DefaultWeapon;
        public WeaponData Shotgut;
        public WeaponData MachineGun;
        public CoreData PlasmaCore;
        public CoreData VoltageCore;

        //*************************

        public static CollectableFactory Instance { private set; get; }

        private void Start()
        {
            Instance = this;
        }

        public ScriptableBase CreateCollectable(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.BUL_BIG: return BigBullet;
                case CollectableType.BUL_BOOM: return ExplodingBullet;
                case CollectableType.GUN_PISTOL: return DefaultWeapon;
                case CollectableType.GUN_SHOTGUT: return Shotgut;
                case CollectableType.GUN_MACHINE_GUN: return MachineGun;
                case CollectableType.CORE_PLASMA: return PlasmaCore;
                case CollectableType.CORE_VOLTAGE: return VoltageCore;

                default: throw new UnityException("No creating for the item type: " + type.ToString() + " defined");
            }
        }





    }
}