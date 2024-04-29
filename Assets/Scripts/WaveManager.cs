using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monsters;
using Structs;
public class WaveManager : Singleton<WaveManager>
{
    public List<WaveInfo> waves;

    public int currentWave;
    public int[] maxMonsterNum;
    public float checkTiming;
    public bool checkMonster;
    public bool waveStarted;
    public bool waveCleared;

    void Start()
    {
        waves = new List<WaveInfo>();
        currentWave = 0;
        checkMonster = false;
        waveCleared = false;
        waveStarted = false;
        waves.Add(JsonFileSystem.Load("jsonWave01"));
        waves.Add(JsonFileSystem.Load("jsonWave02"));
        SpawnManager.Instance.countMaxMonster();
        SpawnManager.Instance.Pooling();
    }

    private void Update()
    {
        if(checkMonster) waveCleared = !SpawnManager.Instance.CheckActiveMonster();
    }

}
