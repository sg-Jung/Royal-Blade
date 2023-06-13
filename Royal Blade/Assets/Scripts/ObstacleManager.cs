using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;
    public static ObstacleManager Instance { get { return instance; } }

    [Header("Object Pooling")]
    public GameObject obstaclePrefab;
    public int poolSize;

    private List<GameObject> poolObjects = new List<GameObject>();

    [Header("Obstacle Area & Obstacle Spawn Point")]
    public BoxCollider obsArea;
    public Transform spawnPoint;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        for(int i =0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            poolObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        // ��Ȱ��ȭ�� ������Ʈ �� ��� ������ ������Ʈ ��ȯ
        for (int i = 0; i < poolObjects.Count; i++)
        {
            if (!poolObjects[i].activeInHierarchy)
            {
                return poolObjects[i];
            }
        }

        // ��� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ� ���, �߰� �����Ͽ� ��ȯ
        GameObject obj = Instantiate(obstaclePrefab);
        obj.SetActive(false);
        poolObjects.Add(obj);
        return obj;
    }

    void SpawnObject()
    {
        GameObject poolObject = GetPooledObject();

        if(poolObject != null)
        {
            poolObject.SetActive(true);
            poolObject.transform.position = spawnPoint.position;
        }
    }

    public void KnockBackObstacle(int index) // index 0: attack, 1: shield
    {
        Collider[] colliders = Physics.OverlapBox(obsArea.bounds.center, obsArea.bounds.extents, Quaternion.identity);

        foreach(Collider collider in colliders)
        {
            Obstacle obs = collider.GetComponent<Obstacle>();
            
            if(obs != null)
            {
                if (index == 0)
                {
                    obs.AttackKnockBack();
                }
                else if (index == 1)
                {
                    obs.ShieldKnockBack();
                }
            }
        }
    }

}
