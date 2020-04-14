using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    CollectableType Type { get; }
    CollectableType Collect();
}

public enum CollectableType
{
    HP_BOBBLE,
    CORE_PLASMA,
    CORE_VOLTAGE,
    BUL_BIG,
    BUL_BOOM,
    BUL_SPRAY,
    GUN_PISTOL,
    GUN_SHOTGUT,
    GUN_MACHINE_GUN
}
