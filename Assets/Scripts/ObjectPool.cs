using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T: Component
{
    private readonly T _prefab;
    private readonly Queue<T> _restingObjects;
    private readonly Transform _container;
    private readonly Action<T> _initializeAction;

    private int _poolSize;

    public ObjectPool(T prefab, int poolSize, Transform container, Action<T> initalizeAction = default)
    {
        _prefab = prefab;
        _restingObjects = new Queue<T>(poolSize);
        _container = container;
        _initializeAction = initalizeAction;
        _poolSize = poolSize;

        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateObject();
        }
    }

    private void EnlargePool()
    {
        CreateObject();
        _poolSize++;
    }

    private void CreateObject()
    {
        T newObject = Object.Instantiate(_prefab, _container);

        newObject.gameObject.SetActive(false);

        _initializeAction?.Invoke(newObject);

        _restingObjects.Enqueue(newObject);
    }

    public T GetFromPool()
    {
        if (_restingObjects.Count == 0)
        {
            EnlargePool();
        }

        T pooledObject = _restingObjects.Dequeue();

        pooledObject.gameObject.SetActive(true);

        return pooledObject;
    }

    public void ReturnToPool(T obj)
    {
        _restingObjects.Enqueue(obj);
    }

}