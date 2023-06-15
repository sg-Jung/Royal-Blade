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

    [Header("Obstacle Info")]
    public ParticleSystem ps;
    public AudioSource audioSrc;
    public int obstacleScore;
    public float health;
    public float maxHealth;

    [Header("Obstacle Area & Obstacle Spawn Point")]
    public BoxCollider obsArea;
    public Transform spawnPoint;

    [Header("Spawn Setting")]
    public float gridZ; // ������ ũ��
    public float spacing; // ���� ���� ����
    public float spawnTime;
    public int spawnMinCount;
    public int spawnMaxCount;
    public int curSpawnCount;

    enum State { Attack, Shield }

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
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            poolObjects.Add(obj);
        }

        StartSpawnObstacle();
    }

    void StartSpawnObstacle()
    {
        StartCoroutine(SpawnObstacleCor());
    }

    IEnumerator SpawnObstacleCor()
    {
        while (true)
        {
            curSpawnCount = Random.Range(spawnMinCount, spawnMaxCount);

            for(int i = 0; i < curSpawnCount; i++)
            {
                SpawnObject();
            }

            yield return new WaitForSeconds(spawnTime);
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

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void SpawnObject()
    {
        GameObject poolObject = GetPooledObject();
        Obstacle obs = poolObject.GetComponent<Obstacle>();
        
        if (obs != null)
        {
            obs.health = health;
        }

        if (poolObject != null)
        {
            poolObject.SetActive(true);
            poolObject.transform.position = GetSpawnPosition();
        }
    }

    Vector3 GetSpawnPosition()
    {
        float zPos = Random.Range(0, gridZ) * spacing;

        // ���� ����Ʈ�� �������� ���� ��ġ�� ��ȯ
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0f, 0f, zPos);

        return spawnPosition;
    }

    public void KnockBackObstacle(int index) // index 0: attack, 1: shield
    {
        Collider[] colliders = Physics.OverlapBox(obsArea.bounds.center, obsArea.bounds.extents, Quaternion.identity);

        foreach(Collider collider in colliders)
        {
            Obstacle obs = collider.GetComponent<Obstacle>();
            
            if(obs != null)
            {
                if (index == (int)State.Attack) // attack
                {
                    obs.AttackKnockBack();
                }
                else if (index == (int)State.Shield) // shield
                {
                    obs.ShieldKnockBack();
                }
            }
        }
    }

    public void ExplosionObstacle(GameObject obj)
    {
        SpawnExplosionParticle(obj);
        ReturnToPool(obj);
        ExplosionSound();
        GameManager.Instance.AddScore(obstacleScore);
        ItemManager.Instance.AddRandomItem();
    }

    public void SpawnExplosionParticle(GameObject obj)
    {
        ParticleSystem explosionPs = Instantiate(ps);
        explosionPs.transform.position = obj.transform.position;
        explosionPs.transform.rotation = Quaternion.identity;

        StartCoroutine(ExplosionPsCor(explosionPs));
    }

    IEnumerator ExplosionPsCor(ParticleSystem ps)
    {
        ps.Play();
        yield return new WaitUntil(() => ps.isPlaying == false);
        ps.Stop();
        Destroy(ps.gameObject);
    }

    public void ExplosionSound()
    {
        if (audioSrc.isPlaying) audioSrc.Stop();
        audioSrc.Play();
    }

}
