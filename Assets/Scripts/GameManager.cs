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

    //UIȰ��ȭ�� bool ����
    public bool waveCleared;

    //MonsterSpawner���� �� ������ false�϶� ���� ���̺��� ���͸� ������.
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
            if (currentWave == winWave) //���̺� Ŭ���� �ߴµ� ������ ���̺� ����.
            {
                if (!isGameWin)
                {
                    Debug.Log("�̰��!");
                    isGameWin = true;
                    StartCoroutine(WinCoroutine());
                }
            }
            else    //���̺� Ŭ���� �ߴµ� ������ ���̺�� �ƴϾ���.
            {
                Debug.Log("���̺� Ŭ����");
                isUnboxing = true;  //���ڱ����.
            }
        }

        if(isWaveClear && !isUnboxing && !isGameWin)  //���̺굵 ���� ���°� �ڽ��� ������
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
            Debug.Log("����!");
            if (isPaused) { Debug.Log("����!"); isPaused = false; }
            else { Debug.Log("�ð�����!"); isPaused = true; }
        }
    }

}