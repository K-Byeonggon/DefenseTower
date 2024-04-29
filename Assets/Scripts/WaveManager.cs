using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monsters;
using Structs;
public class WaveManager : Singleton<WaveManager>
{
    public List<WaveInfo> waves;
    public int currentWave = 0;
    public int[] maxMonsterNum;

    void Start()
    {
        waves = new List<WaveInfo>();
        waves.Add(JsonFileSystem.Load("jsonWave01"));
        waves.Add(JsonFileSystem.Load("jsonWave02"));
        SpawnManager.Instance.countMaxMonster();
        SpawnManager.Instance.Pooling();
    }


}
