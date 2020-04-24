using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Items
{
    public class PlasmaCore : CollectableBase
    {
        void Start()
        {
            mType = CollectableType.CORE_PLASMA;
        }
    }
}