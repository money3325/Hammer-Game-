

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomizedCharacter : ScriptableObject
{
    [field: SerializeField]
    public List<CustomzationDate> Data { get; private set; }

   
}