using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private float initialYpos;
    public float frequency = 0.5f; // 사인 곡선의 주파수 (주기의 역수)
    public float amplitude = 20f; // 사인 곡선의 진폭
    private float time = 0f; // 경과 시간
    public float rotationSpeed = 360f; // 1초에 회전할 각도

    private float elapsedTime = 0f; // 경과 시간
    private float turnTime = 0f;

    private void Start()
    {
        initialYpos = transform.position.y;
    }

    private void UpDownTitle()
    {
        // 사인 함수 계산하여 y 위치 조정
        float yPos = initialYpos + Mathf.Sin(2 * Mathf.PI * frequency * time) * amplitude;
        // 현재 위치 업데이트
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

    }

    private void TurnTitle()
    {
        elapsedTime += Time.deltaTime;
        
        //시간이 3초가 지나면 돌기 시작하기
        if(elapsedTime > 3f)
        {
            transform.Rotate(Vector3.back, Time.deltaTime*720f);
            turnTime += Time.deltaTime;
        }

        //한바퀴를 돌았으면 돌기 멈추기
        if (turnTime >= 0.5f)
        {
            elapsedTime = 0;
            turnTime = 0;
        }

    }


    private void Update()
    {
        // 시간 업데이트
        time += Time.deltaTime;
        UpDownTitle();
        TurnTitle();
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
