using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnums;

public class MonsterSpawner : MonoBehaviour
{
    Dictionary<string, int> monsterIndex = new Dictionary<string, int>();
    private void Start()
    {
        monsterIndex.Add("Swordknight", 0);
        monsterIndex.Add("Banshee", 1);
        monsterIndex.Add("RedBat", 2);
        monsterIndex.Add("Golem", 3);

    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameState.InWave)
        {
            if (!WaveManager.Instance.waveStarted) { WaveManager.Instance.waveStarted = true; StartWave(); }
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
        GameObject monster = WaveManager.Instance.monsterPools[monsterIndex[name]].Dequeue();
        WaveManager.Instance.monsterPools[monsterIndex[name]].Enqueue(monster);
        Debug.Log(monster == null);
        monster.SetActive(true);
        monster.transform.position = transform.GetChild(point).position;
    }

}
