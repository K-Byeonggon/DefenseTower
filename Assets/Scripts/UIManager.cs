using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StateEnums;

public class UIManager : MonoBehaviour
{
    public GameObject waveClearUI;
    public GameObject winUI;
    public GameObject gameOverUI;
    public GameObject boxUI;
    public GameObject pauseUI;
    public GameObject itemBox;

    private void Start()
    {
        waveClearUI.SetActive(false);
        winUI.SetActive(false);
        gameOverUI.SetActive(false);
        boxUI.SetActive(false);
        pauseUI.SetActive(false);
    }



    private void Update()
    {
        if(GameManager.Instance.currentState == GameState.GameLose) { gameOverUI.SetActive(true); }
        else { gameOverUI.SetActive(false); }
        if(GameManager.Instance.currentState == GameState.GameWin) { winUI.SetActive(true); }
        else {  winUI.SetActive(false); }
        if(GameManager.Instance.currentState == GameState.WaveCleared)
        {
            waveClearUI.transform.GetChild(0).GetComponent<Text>().text = string.Format("Wave {0:D2} Clear", WaveManager.Instance.currentWave); 
                waveClearUI.SetActive(true); 
        }
        else {  waveClearUI.SetActive(false); }
        if(GameManager.Instance.isPaused) { pauseUI.SetActive(true); }
        else { pauseUI.SetActive(false); }
        if(GameManager.Instance.currentState == GameState.UnBoxing) { itemBox.SetActive(true); }
        else { itemBox.SetActive(false); }
    }
}
