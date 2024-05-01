using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnums;

public class MonsterSpawner : MonoBehaviour
{

    public GameObject[] monsterPrefabs;
    public int[] poolSizes;
    public Queue<GameObject>[] monsterPools;
    public GameObject monsterPool;

    Dictionary<string, int> monsterIndex = new Dictionary<string, int>();
    private void Start()
    {
        monsterIndex.Add("Swordknight", 0);
        monsterIndex.Add("Banshee", 1);
        monsterIndex.Add("RedBat", 2);
        monsterIndex.Add("Golem", 3);

        monsterPool = GameObject.FindWithTag("Pool");
        if (monsterPool != null) Debug.Log("Ã£À½");

        monsterPools = new Queue<GameObject>[monsterPrefabs.Length];
        for (int index = 0; index < monsterPools.Length; index++)
        {
            monsterPools[index] = new Queue<GameObject>();
        }

        poolSizes = new int[monsterPrefabs.Length];

        countMaxMonster();
        Pooling();

    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameState.InWave)
        {
            if (!WaveManager.Instance.waveStarted) { WaveManager.Instance.waveStarted = true; StartWave(); }
        }
    }

    public void countMaxMonster()
    {
        foreach (WaveInfo wave in WaveManager.Instance.waves)
        {
            int[] monsterNum = new int[monsterPrefabs.Length];
            monsterNum[0] = wave.countMonster("Swordknight");
            monsterNum[1] = wave.countMonster("Banshee");
            monsterNum[2] = wave.countMonster("RedBat");
            monsterNum[3] = wave.countMonster("Golem");

            for (int i = 0; i < monsterNum.Length; i++)
            {
                if (poolSizes[i] < monsterNum[i]) poolSizes[i] = monsterNum[i];
            }
        }
        Debug.Log($"{poolSizes[0]}, {poolSizes[1]}, {poolSizes[2]}, {poolSizes[3]}");
    }

    public void Pooling()
    {
        for (int i = 0; i < poolSizes.Length; i++)
        {
            for (int k = 0; k < poolSizes[i]; k++)
            {
                GameObject monster = Instantiate(monsterPrefabs[i], new Vector3(0, -10, 0), Quaternion.identity);
                monster.SetActive(false);
                monster.transform.SetParent(monsterPool.transform.GetChild(i));
                monsterPools[i].Enqueue(monster);
            }
        }
    }

    private void StartWave()
    {
        int currentWaveIndex = WaveManager.Instance.currentWave;
        WaveInfo currentWave = WaveManager.Instance.waves[currentWaveIndex];
        WaveManager.Instance.checkTiming = currentWave.wave[currentWave.wave.Count - 1].time;
        foreach (SpawnInfo spawn in currentWave.wave)
        {
            StartCoroutine(SpawnCoroutine(spawn.time, spawn.point, spawn.name));
        }
        StartCoroutine(ActiveClearCheck());
    }

    private IEnumerator ActiveClearCheck()
    {
        yield return new WaitForSeconds(WaveManager.Instance.checkTiming);
        WaveManager.Instance.checkMonster = true;
    }

    private IEnumerator SpawnCoroutine(float time, int point, string name)
    {
        yield return new WaitForSeconds(time);
        GameObject monster = monsterPools[monsterIndex[name]].Dequeue();
        monsterPools[monsterIndex[name]].Enqueue(monster);
        Debug.Log(monster == null);
        monster.SetActive(true);
        monster.transform.position = transform.GetChild(point).position;
    }

}
