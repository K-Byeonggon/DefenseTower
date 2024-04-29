using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveInfo
{
    public List<SpawnInfo> wave;

    public int countMonster(string name)
    {
        int count = 0;
        foreach (SpawnInfo spawn in wave)
        {
            if(spawn.name == name) count++;
        }
        return count;
    }
}
