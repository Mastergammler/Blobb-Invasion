
using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using BlobbInvasion.UI;

namespace BlobbInvasion.Utilities
{
    public static class Utils
    {
        public static CraftingType ConvertCollectableType(CollectableType collType)
        {
            switch (collType)
            {
                case CollectableType.BUL_BIG:
                case CollectableType.BUL_BOOM:
                case CollectableType.BUL_SPRAY: return CraftingType.BULLET;
                case CollectableType.CORE_PLASMA:
                case CollectableType.CORE_VOLTAGE: return CraftingType.CORE;
                case CollectableType.GUN_MACHINE_GUN:
                case CollectableType.GUN_PISTOL:
                case CollectableType.GUN_SHOTGUT: return CraftingType.WEAPON;
                default: throw new NotImplementedException("No handling implemented for type: " + collType.ToString());
            }
        }
    }

}
