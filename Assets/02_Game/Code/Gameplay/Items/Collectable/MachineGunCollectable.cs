using UnityEngine;

namespace BlobbInvasion.Gameplay.Items
{
    public class MachineGunCollectable : CollectableBase
    {
        private void Start()
        {
            mType = CollectableType.GUN_MACHINE_GUN;
        }

    }
}