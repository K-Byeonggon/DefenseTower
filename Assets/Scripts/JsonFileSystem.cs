using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class JsonFileSystem
{
    public static WaveInfo Load(string jsonFileName)
    {
        string filePath = "Assets/" + jsonFileName + ".json";

        if (!File.Exists(filePath))
        {
            Debug.LogError("파일 없거나 못찾음");
            return null;
        }
        else { Debug.Log("파일 읽는데 성공해써~"); }

        string saveFile = File.ReadAllText(filePath);
        Debug.Log(saveFile);
        WaveInfo waveInfo = JsonUtility.FromJson<WaveInfo>(saveFile);

        return waveInfo;
    }
}
