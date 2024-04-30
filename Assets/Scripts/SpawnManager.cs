using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject[] monsterPrefabs;
    public Queue<GameObject>[] monsterPools;
    public int[] poolSizes;
    public GameObject monsterPool;

    private void Start()
    {
        monsterPool = GameObject.FindWithTag("Pool");
        if (monsterPool != null) Debug.Log("Ã£À½");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") SetDefault();
    }

    private void SetDefault()
    {
        monsterPool = GameObject.FindWithTag("Pool");
    }

    public void countMaxMonster()
    {
        poolSizes = new int[monsterPrefabs.Length];
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
        monsterPools = new Queue<GameObject>[monsterPrefabs.Length];
        for (int index = 0; index < monsterPools.Length; index++)
        {
            monsterPools[index] = new Queue<GameObject>();
        }

        for (int i = 0; i < poolSizes.Length; i++)
        {
            for (int k = 0; k < poolSizes[i]; k++)
            {
                GameObject monster = Instantiate(monsterPrefabs[i], new Vector3(0, -10, 0), Quaternion.identity);
                monster.SetActive(false);
                monster.transform.parent = monsterPool.transform.GetChild(i);
                monsterPools[i].Enqueue(monster);
            }
        }
    }

    public bool CheckActiveMonster()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            for(int k = 0; k < transform.GetChild(i).childCount; k++)
            {
                if (monsterPool.transform.GetChild(i).GetChild(k).gameObject.activeSelf) return true;
            }
        }
        return false;
    }
}
