using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

   

    [SerializeField] private int poolSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();


    [Header("To Initialize")]

    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject ammoPickup;
    


    private void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }

       
    }

    private void Start() {
        InitializeNewPool(weaponPickup);
        InitializeNewPool(ammoPickup);
    }
    

    public void InitializeNewPool(GameObject prefab){

        poolDictionary[prefab] = new Queue<GameObject>();
        for(int i = 0 ; i < poolSize; i++)
        {
            CreateNewObject(prefab);

        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newobject = Instantiate(prefab, transform);
        newobject.AddComponent<PoolObject>().originalPrefab = prefab;
        newobject.SetActive(false);

        poolDictionary[prefab].Enqueue(newobject);
    }

    private void ReturnToPool(GameObject objectToReturn){
        GameObject originalPrefab = objectToReturn.GetComponent<PoolObject>().originalPrefab;

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);

    }
    public void ReturnToObject(GameObject objectToReturn, float delay = .001f) => StartCoroutine(DelayReturn(delay,objectToReturn));

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn){
        yield return new WaitForSeconds(delay);
        ReturnToPool(objectToReturn);
    }





    public GameObject GetObject(GameObject prefab){

        if(poolDictionary.ContainsKey(prefab) == false){
            InitializeNewPool(prefab);
        }
        if(poolDictionary[prefab].Count == 0){
            CreateNewObject(prefab); // if all the the prefab in the queue is using create a new one;
        }

        GameObject objectToGet = poolDictionary[prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;
        return objectToGet;

    }
}
