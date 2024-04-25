using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Monsters;
using Structs;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private int wave;
    const int MONSTER_TYPE = 4;
    public GameObject[] monsterPrefabs = new GameObject[MONSTER_TYPE];
    private int[] poolCount = { 10, 10, 16, 4, 3 };
    [SerializeField] private Queue<GameObject>[] monsterPools;
    [SerializeField] private bool[] monsterSpawning;
    [SerializeField] private bool waveStarted;

    private void OnEnable()
    {
        wave = GameManager.Instance.currentWave;    
    }

    private void Start()
    {
        wave = GameManager.Instance.currentWave;
        monsterSpawning = new bool[transform.childCount];
        monsterPools = new Queue<GameObject>[MONSTER_TYPE];
        for(int i = 0; i < transform.childCount; i++)
        {
            monsterSpawning[i] = false;
        }
        for(int i = 0; i < MONSTER_TYPE; i++)
        {
            monsterPools[i] = new Queue<GameObject>();
        }
        for(int k = 0; k < MONSTER_TYPE; k++)
        {
            for (int i = 0; i < poolCount[k]; i++)
            {
                GameObject monster = Instantiate(monsterPrefabs[k], new Vector3(0, -20, 0), Quaternion.identity);
                monster.SetActive(false);
                monsterPools[k].Enqueue(monster);
            }
        }
    }


    private void Update()
    {
        if (!GameManager.Instance.waveStarted) 
        { 
            StartCurrentWave();
            GameManager.Instance.waveStarted = true;
        }
    }

    private void StartCurrentWave()
    {
        switch (GameManager.Instance.currentWave)
        {
            case 1:
                StartWave(WaveManager.Instance.wave01);
                break;
            case 2:
                StartWave(WaveManager.Instance.wave02);
                break;
            case 3:
                StartWave(WaveManager.Instance.wave03);
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }
    }

    private void StartWave(List<MonsterSpawnInfo> wave) 
    {
        foreach(MonsterSpawnInfo spawnInfo in wave)
        {
            StartCoroutine(DelayedSpawn(spawnInfo.spawnTime, spawnInfo.spawnCount, spawnInfo.monsterName, spawnInfo.spawnPoint, spawnInfo.spawnDelay));
        }
    }

    private IEnumerator DelayedSpawn(float nextSpawnDelay, int MaxSpawn, MonsterName monsterName, int spawnPoint, float delay)
    {
        yield return new WaitForSeconds(nextSpawnDelay);
        StartCoroutine(SpawnCoroutine(MaxSpawn, monsterName, spawnPoint, delay));
    }

    private IEnumerator SpawnCoroutine(int MaxSpawn, MonsterName monsterName, int spawnPoint, float delay)
    {
        int currentMonsterSpawn = 0;
        while (currentMonsterSpawn < MaxSpawn)
        {
            monsterSpawning[spawnPoint] = true;
            GameObject monster = monsterPools[(int)monsterName].Dequeue();
            monsterPools[(int)monsterName].Enqueue(monster);
            monster.SetActive(true);
            monster.transform.position = gameObject.transform.GetChild(spawnPoint).position;

            yield return new WaitForSeconds(delay);
            monsterSpawning[spawnPoint] = false;
            currentMonsterSpawn++;
        }
    }
}
