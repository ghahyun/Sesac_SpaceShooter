using System;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    //Pool 변수 선언
    public IObjectPool<Bullet> bulletPool;
    public GameObject bulletPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        //ObjectPool 초기화
        bulletPool = new ObjectPool<Bullet>
        (
            createFunc: CreateItem,
            actionOnGet: OnTakeItem,
            actionOnRelease: OnReturnItem,
            actionOnDestroy: OnDestroyItem,
            defaultCapacity: 5,
            maxSize: 10,
            collectionCheck: false
        );
    }

    private Bullet CreateItem()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        return bullet;
    }

    private void OnTakeItem(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnItem(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyItem(Bullet bullet)
    {
        Debug.Log("초과 아이템 삭제함");
        Destroy(bullet.gameObject);
    }
}
