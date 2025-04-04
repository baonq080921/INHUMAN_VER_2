using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<GameObject> bulletPools;


    private void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }

       
    }
    void Start()
    {
        bulletPools = new Queue<GameObject>();
        CreateInitialPool();
    }
    

    public void CreateInitialPool(){
        for(int i = 0 ; i < poolSize; i++)
        {
            CreateBullet();

        }
    }

    private void CreateBullet()
    {
        GameObject obj = Instantiate(bulletPrefab, transform);
        obj.SetActive(false);
        bulletPools.Enqueue(obj);
    }

    public void ReturnBullet(GameObject bullet){
        bullet.SetActive(false);
        bulletPools.Enqueue(bullet);
        bullet.transform.parent = transform;

    }


    public GameObject GetBullet(){

        if(bulletPools.Count == 0) CreateBullet();

        GameObject bulletToGet = bulletPools.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;
        return bulletToGet;

    }
}
