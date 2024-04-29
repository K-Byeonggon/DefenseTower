using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnInfo
{
    public float time;
    public int point;
    public string name;

    public SpawnInfo(float time, int point, string name)
    {
        this.time = time;
        this.point = point;
        this.name = name;
    }
}
