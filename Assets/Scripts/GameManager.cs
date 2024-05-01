using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using StateEnums;

public class GameManager : Singleton<GameManager>
{
    public GameState currentState;
    public Camera mainCamera;
    public float towerMaxHp = 500;
    public float towerHp = 500;
    public int winWave = 2;
    public bool isPaused;
    public bool isCoroutine = false;
    public bool isGameOver = false;

    private void Start()
    {
        currentState = GameState.InWave;

        isPaused = false;

        towerMaxHp = 500;
        towerHp = towerMaxHp;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SampleScene") SetDefault();
    }

    private void SetDefault()
    {
        mainCamera = Camera.main;
        towerHp = towerMaxHp;
        currentState = GameState.InWave;
        isCoroutine = false;
        isGameOver = false;
        isCoroutine = false;
    }


    public bool InCamera(GameObject gameObject)
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(gameObject.transform.position);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 ||
            viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            return false;
        }
        else return true;
    }


    private void Update()
    {
        //일시 정지
        if(isPaused) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        //Debug.Log(currentState);
        switch(currentState)
        {
            case GameState.InWave:
                InWave(); break;
            case GameState.WaveCleared:
                WaveCleared(); break;
            case GameState.UnBoxing:
                UnBoxing(); break;
            case GameState.GameWin:
                GameWin(); break;
            case GameState.GameLose:
                GameLose(); break;

        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void InWave()
    {
        if (towerHp <= 0)
        {
            ChangeState(GameState.GameLose);
            return;
        }
        if (WaveManager.Instance.waveCleared)
        {
            Debug.Log("curWave: " + WaveManager.Instance.currentWave);
            Debug.Log("winWave: " + winWave);
            if (WaveManager.Instance.currentWave == winWave)
            {
                Debug.Log("게임 승리 상태");
                ChangeState(GameState.GameWin);
            }
            else
            {
                Debug.Log("웨이브 클리어 상태");
                WaveManager.Instance.currentWave++;
                ChangeState(GameState.WaveCleared);
            }
        }
    }

    private IEnumerator WaveClearCoroutine()
    {
        isCoroutine = true;
        yield return new WaitForSeconds(3f);
        WaveManager.Instance.checkMonster = false;
        WaveManager.Instance.waveCleared = false;
        WaveManager.Instance.waveStarted = false;
        ChangeState(GameState.UnBoxing);
        isCoroutine = false;

    }
    public void WaveCleared()
    {
        if(!isCoroutine) StartCoroutine(WaveClearCoroutine());
    }

    public void UnBoxing()
    {

    }

    private IEnumerator WinCoroutine()
    {
        isCoroutine = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Title");
    }

    public void GameWin()
    {
        if(!isCoroutine) { isCoroutine = true; StartCoroutine(WinCoroutine()); }
    }

    public void GameLose()
    {
        if (!isGameOver) { isGameOver = true; }
    }

    public void SetTowerHp(float delta)
    {
        towerHp += delta;
    }

    private void OnPause(InputValue inputValue)
    {
        if (inputValue.Get() != null)
        {
            if (isPaused) { isPaused = false; }
            else { isPaused = true; }
        }
    }

}