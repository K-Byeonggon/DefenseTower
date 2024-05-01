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


    public GameObject monsterPool;

    void Awake()
    {

        waves = new List<WaveInfo>();



        currentWave = 0;
        checkMonster = false;
        waveCleared = false;
        waveStarted = false;
        waves.Add(JsonFileSystem.Load("jsonWave01"));
        waves.Add(JsonFileSystem.Load("jsonWave02"));
        waves.Add(JsonFileSystem.Load("jsonWave03"));


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") SetDefault();
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
        if (monsterPool != null) Debug.Log("Ã£À½");
    }

    private void Update()
    {
        if (GameManager.Instance.currentState != GameState.InWave) checkMonster = false;
        if (checkMonster) waveCleared = !CheckActiveMonster();
    }

}
