using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monsters;
using Structs;
public class WaveManager : Singleton<WaveManager>
{
    public int[] enemyCounts;

    public List<MonsterSpawnInfo> wave01;
    public List<MonsterSpawnInfo> wave02;
    public List<MonsterSpawnInfo> wave03;
    public List<MonsterSpawnInfo> wave04;
    public List<MonsterSpawnInfo> wave05;

    public void Start()
    {
        enemyCounts = new int[6];//0¹ø ÀÎµ¦½º ºñ¿öµÒ
        wave01 = new List<MonsterSpawnInfo>();
        wave02 = new List<MonsterSpawnInfo>();
        wave03 = new List<MonsterSpawnInfo>();
        wave04 = new List<MonsterSpawnInfo>();
        wave05 = new List<MonsterSpawnInfo>();

        //SetTestWave();
        //SetTestWave2();
        //SetTestWave3();
        SetWave01();
        SetWave02();
        SetWave03();

    }

    public void AddSpawnInfo(int waveNum, float spawnTime, int spawnCount, MonsterName monsterName, int spawnPoint, float spawnDelay)
    {
        MonsterSpawnInfo spawnInfo = new MonsterSpawnInfo();
        spawnInfo.spawnTime = spawnTime;
        spawnInfo.spawnCount = spawnCount;
        spawnInfo.monsterName = monsterName;
        spawnInfo.spawnPoint = spawnPoint;
        spawnInfo.spawnDelay = spawnDelay;

        enemyCounts[waveNum] += spawnCount;

        switch (waveNum)
        {
            case 1:
                wave01.Add(spawnInfo);
                break;
            case 2: 
                wave02.Add(spawnInfo);
                break;
            case 3:
                wave03.Add(spawnInfo);
                break;
            case 4:
                wave04.Add(spawnInfo);
                break;
            case 5:
                wave05.Add(spawnInfo);
                break;
            default:
                break;
        }
    }

    public void SetTestWave()
    {
        AddSpawnInfo(1, 0f, 1, MonsterName.SwordKnight, 0, 3f);
    }

    public void SetTestWave2()
    {
        AddSpawnInfo(2, 0f, 1, MonsterName.Banshee, 1, 3f);
    }

    public void SetTestWave3()
    {
        AddSpawnInfo(3, 0f, 1, MonsterName.Golem, 0, 3f);
    }

    public void SetWave01()
    {
        AddSpawnInfo(1, 0f, 5, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(1, 0f, 3, MonsterName.Banshee, 1, 5f);
        AddSpawnInfo(1, 20f, 5, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(1, 20f, 3, MonsterName.Banshee, 2, 5f);
        AddSpawnInfo(1, 40f, 3, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(1, 40f, 2, MonsterName.Banshee, 1, 5f);
        AddSpawnInfo(1, 40f, 3, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(1, 40f, 2, MonsterName.Banshee, 2, 5f);
    }

    public void SetWave02()
    {
        AddSpawnInfo(2, 0f, 5, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(2, 0f, 3, MonsterName.RedBat, 1, 5f);
        AddSpawnInfo(2, 20f, 5, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(2, 20f, 3, MonsterName.RedBat, 2, 5f);
        AddSpawnInfo(2, 40f, 3, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(2, 40f, 2, MonsterName.RedBat, 1, 5f);
        AddSpawnInfo(2, 40f, 3, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(2, 40f, 2, MonsterName.RedBat, 2, 5f);
    }

    public void SetWave03()
    {
        AddSpawnInfo(3, 0f, 2, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(3, 0f, 1, MonsterName.Golem, 0, 10f);
        AddSpawnInfo(3, 0f, 2, MonsterName.Banshee, 1, 5f);
        AddSpawnInfo(3, 20f, 5, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(3, 20f, 1, MonsterName.Golem, 3, 10f);
        AddSpawnInfo(3, 20f, 2, MonsterName.Banshee, 2, 5f);
        AddSpawnInfo(3, 40f, 2, MonsterName.SwordKnight, 0, 3f);
        AddSpawnInfo(3, 40f, 1, MonsterName.Banshee, 1, 5f);
        AddSpawnInfo(3, 40f, 1, MonsterName.Golem, 0, 10f);
        AddSpawnInfo(3, 40f, 2, MonsterName.SwordKnight, 3, 3f);
        AddSpawnInfo(3, 40f, 1, MonsterName.Banshee, 2, 5f);
        AddSpawnInfo(3, 40f, 1, MonsterName.Golem, 3, 10f);
    }
}
