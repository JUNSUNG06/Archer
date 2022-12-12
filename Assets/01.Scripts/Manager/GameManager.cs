using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<PoolableMono> poolingList = new List<PoolableMono>();

    private void Awake()
    {
        Instance = this;
        PoolManager.Instance = new PoolManager(this.transform);

        foreach(PoolableMono p in poolingList)
        {
            PoolManager.Instance.CreatePool(p, 30);
        }
    }
}
