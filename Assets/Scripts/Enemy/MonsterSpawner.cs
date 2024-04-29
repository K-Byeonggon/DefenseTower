using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Monsters;
using Structs;

public class MonsterSpawner : MonoBehaviour
{
    Dictionary<string, int> monsterIndex = new Dictionary<string, int>();

    private void Start()
    {
        monsterIndex.Add("Swordknight", 0);
        monsterIndex.Add("Banshee", 1);
        monsterIndex.Add("RedBat", 2);
        monsterIndex.Add("Golem", 3);

        StartWave();
    }



    private void StartWave()
    {
        WaveInfo currentWave = WaveManager.Instance.waves[WaveManager.Instance.currentWave];

        foreach(SpawnInfo spawn in currentWave.wave)
        {
            StartCoroutine(SpawnCoroutine(spawn.time, spawn.point, spawn.name));
        }
    }

    private IEnumerator SpawnCoroutine(float time, int point, string name)
    {
        yield return new WaitForSeconds(time);
        GameObject monster =  SpawnManager.Instance.monsterPools[monsterIndex[name]].Dequeue();
        SpawnManager.Instance.monsterPools[monsterIndex[name]].Enqueue(monster);
        monster.SetActive(true);
        monster.transform.position = transform.GetChild(point).position;
    }



}
