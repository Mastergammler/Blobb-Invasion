﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Blobble : CollectableBase
{
    void Start()
    {
        mType = CollectableType.HP_BOBBLE;
    }
}
