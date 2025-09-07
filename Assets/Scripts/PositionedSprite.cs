using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class positionedSprite
{
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    [field: SerializeField]
    public Vector3 Position{ get; set; }
}