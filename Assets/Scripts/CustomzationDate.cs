using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomzationDate
{
    [field: SerializeField]
    public CustomizationType Type { get; private set; }
    [field: SerializeField]
    public positionedSprite Sprite { get; private set; }
    public CustomzationDate(CustomizationType t, positionedSprite s)
    {
        Type = t;
        Sprite = s;
    }
}