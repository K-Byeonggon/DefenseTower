using System.Collections;
using System.Collections.Generic;
using StateEnums;
using UnityEngine.SceneManagement;
using UnityEngine;
public class WaveManager : Singleton<WaveManager>
{
    public List<WaveInfo> waves;

    public int currentWave;
    public int[] maxMonsterNum;
    public float checkTiming;
    public bool checkMonster;
    public bool waveStarted;
    public bool waveCleared;

    public GameObject[] monsterPrefabs;
    public Queue<GameObject>[] monsterPools;
    public int[] poolSizes;
    public GameObject monsterPool;


    void Start()
    {
        monsterPool = GameObject.FindWithTag("Pool");
        if (monsterPool != null) Debug.Log("찾음");

        waves = new List<WaveInfo>();

        monsterPools = new Queue<GameObject>[monsterPrefabs.Length];
        for (int index = 0; index < monsterPools.Length; index++)
        {
            monsterPools[index] = new Queue<GameObject>();
        }

        poolSizes = new int[monsterPrefabs.Length];

        currentWave = 0;
        checkMonster = false;
        waveCleared = false;
        waveStarted = false;
        waves.Add(JsonFileSystem.Load("jsonWave02"));
        waves.Add(JsonFileSystem.Load("jsonWave02"));
        countMaxMonster();
        Pooling();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") SetDefault();
    }

    public void countMaxMonster()
    {
        foreach (WaveInfo wave in waves)
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

    public bool CheckActiveMonster()
    {
        for (int i = 0; i < monsterPool.transform.childCount; i++)
        {
            for (int k = 0; k < monsterPool.transform.GetChild(i).childCount; k++)
            {
                if (monsterPool.transform.GetChild(i).GetChild(k).gameObject.activeSelf) return true;
            }
        }
        return false;
    }


    private void SetDefault()
    {
        checkMonster = false;
        waveCleared = false;
        waveStarted = false;
        currentWave = 0;

        monsterPool = GameObject.FindWithTag("Pool");
        if (monsterPool != null) Debug.Log("찾음");
        countMaxMonster();
        Pooling();
    }

    private void Update()
    {
        if (GameManager.Instance.currentState != GameState.InWave) checkMonster = false;
        if (checkMonster) waveCleared = !CheckActiveMonster();
    }

}
