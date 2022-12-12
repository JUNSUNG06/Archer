using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private Stack<T> pool = new Stack<T>();
    private T _prefab;
    private Transform _parentTrm;

    public Pool(T prefab, Transform parentTrm, int cnt = 10)
    {
        _prefab = prefab;
        _parentTrm = parentTrm;

        for(int i = 0; i < cnt; i++)
        {
            T obj = GameObject.Instantiate(_prefab, _parentTrm);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", ""); 
            obj.gameObject.SetActive(false); 
            pool.Push(obj);
        }
    }

    public T Pop()
    {
        T obj = null; 

        if(pool.Count <= 0)
        {
            obj = GameObject.Instantiate(_prefab, _parentTrm);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
        }
        else
        {
            obj = pool.Pop();
            obj.gameObject.SetActive(true);
        }

        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
}
