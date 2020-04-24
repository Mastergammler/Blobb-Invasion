using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Items
{
    public class ExplodingBullet : CollectableBase
    {
        // Start is called before the first frame update
        void Start()
        {
            mType = CollectableType.BUL_BOOM;
        }
    }
}