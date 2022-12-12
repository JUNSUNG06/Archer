using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public abstract void Reset(Vector3 Position, Quaternion Rotation);
}
