using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public GameObject waveClearUI;
    public GameObject winUI;
    public GameObject gameOverUI;
    public GameObject itemBox;
    public GameObject boxUI;
    public GameObject pauseUI;
    public GameObject TestUI;
    private void Start()
    {
        waveClearUI.SetActive(false);
        winUI.SetActive(false);
        gameOverUI.SetActive(false);
        itemBox.SetActive(false);
        boxUI.SetActive(false);
        pauseUI.SetActive(false);
    }


    private IEnumerator WaveClearUICoroutine()
    { 
        waveClearUI.transform.GetChild(0).GetComponent<Text>().text = string.Format("Wave {0:D2} Clear", GameManager.Instance.currentWave);
        waveClearUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        waveClearUI.SetActive(false);
        itemBox.SetActive(true);
    }


    private void Update()
    {
        if(GameManager.Instance.isGameOver) { gameOverUI.SetActive(true); }
        if(GameManager.Instance.isGameWin) { winUI.SetActive(true); }
        if(GameManager.Instance.waveCleared && !GameManager.Instance.isGameWin) 
        {
            GameManager.Instance.waveCleared = false;
            StartCoroutine(WaveClearUICoroutine());
        }
        if(!GameManager.Instance.isUnboxing) { itemBox.SetActive(false); }
        if(GameManager.Instance.isPaused) { pauseUI.SetActive(true); }
        else { pauseUI.SetActive(false); }
    }
}
