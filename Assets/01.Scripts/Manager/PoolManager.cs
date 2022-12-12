using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using asdkasd;

public class PoolManager
{
    public static PoolManager Instance;

    private Dictionary<string, Pool<PoolableMono>> _pools = new Dictionary<string, Pool<PoolableMono>>();
    private Transform _parentTrm;

    public PoolManager(Transform parentTrm)
    {
        _parentTrm = parentTrm;
    }

    public void CreatePool(PoolableMono prefab, int cnt = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, _parentTrm, cnt);
        _pools.Add(prefab.gameObject.name, pool);
    }

    public PoolableMono Pop(string prefabName, Vector3 Pos, Quaternion Rot)
    {   
        if(_pools.ContainsKey(prefabName) == false)
        {
            Debug.LogError("Ç® ¾øÀ½");
            return null;
        }

        PoolableMono item = _pools[prefabName].Pop();
        item.Reset(Pos, Rot);
        return item;
    }

    public void Push(PoolableMono obj)
    {
        obj.gameObject.SetActive(false);
        _pools[obj.gameObject.name].Push(obj);
    }
}
