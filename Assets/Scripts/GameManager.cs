using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    public bool isGameOver;
    public bool isGameWin;
    public bool isWaveClear;
    public bool isUnboxing;
    public int currentWave = 1;
    public int winWave = 3;
    public Camera mainCamera;
    public float towerMaxHp = 500;
    public float towerHp;
    public int defeatedEnemyCount;

    //UI활성화용 bool 변수
    public bool waveCleared;

    //MonsterSpawner에서 이 변수가 false일때 현재 웨이브의 몬스터를 스폰함.
    public bool waveStarted;

    public bool isPaused;

    private void Start()
    {
        isUnboxing = false;
        isGameWin = false;
        isGameOver = false;
        isWaveClear = false;
        waveStarted = false;
        waveCleared = false;
        towerHp = towerMaxHp;
        defeatedEnemyCount = 0;
        isPaused = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetDefault();
    }

    private void SetDefault()
    {
        mainCamera = Camera.main;
        isGameOver = false;
        isGameWin = false;
        isWaveClear = false;
        waveStarted = false;
        waveCleared = false;
        isUnboxing = false;
        towerHp = towerMaxHp;
        defeatedEnemyCount = 0;
        currentWave = 1;
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

    private IEnumerator NextWaveCoroutine()
    {
        isWaveClear = false;
        defeatedEnemyCount = 0;
        currentWave++;
        yield return new WaitForSeconds(3f);
        waveStarted = false;

    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Title");
    }

    private void Update()
    {
        if(isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (towerHp <= 0 && !isGameOver)
        {
            isGameOver = true;
        }

        else if(defeatedEnemyCount == WaveManager.Instance.enemyCounts[currentWave] && !isWaveClear)
        {
            isWaveClear = true;
            waveCleared = true;
            if (currentWave == winWave) //웨이브 클리어 했는데 마지막 웨이브 였음.
            {
                if (!isGameWin)
                {
                    Debug.Log("이겼다!");
                    isGameWin = true;
                    StartCoroutine(WinCoroutine());
                }
            }
            else    //웨이브 클리어 했는데 마지막 웨이브는 아니었음.
            {
                Debug.Log("웨이브 클리어");
                isUnboxing = true;  //상자까야지.
            }
        }

        if(isWaveClear && !isUnboxing && !isGameWin)  //웨이브도 끝난 상태고 박스도 깠으면
        {
            StartCoroutine(NextWaveCoroutine());
        }
    }

    public void SetTowerHp(float delta)
    {
        towerHp += delta;
    }

    public void WaveUp()
    {
        currentWave++;
    }

    private void OnPause(InputValue inputValue)
    {
        if (inputValue.Get() != null)
        {
            Debug.Log("눌림!");
            if (isPaused) { Debug.Log("해제!"); isPaused = false; }
            else { Debug.Log("시간정지!"); isPaused = true; }
        }
    }

}