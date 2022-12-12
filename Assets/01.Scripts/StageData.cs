using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    [SerializeField] private float maxX;
    [SerializeField] private float minX;

    public float MaxX => maxX;
    public float MinX => minX;
}
