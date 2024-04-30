using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private float initialYpos;
    public float frequency = 0.5f; // ���� ��� ���ļ� (�ֱ��� ����)
    public float amplitude = 20f; // ���� ��� ����
    private float time = 0f; // ��� �ð�
    public float rotationSpeed = 360f; // 1�ʿ� ȸ���� ����

    private float elapsedTime = 0f; // ��� �ð�
    private float turnTime = 0f;

    private void Start()
    {
        initialYpos = transform.position.y;
    }

    private void UpDownTitle()
    {
        // ���� �Լ� ����Ͽ� y ��ġ ����
        float yPos = initialYpos + Mathf.Sin(2 * Mathf.PI * frequency * time) * amplitude;
        // ���� ��ġ ������Ʈ
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

    }

    private void TurnTitle()
    {
        elapsedTime += Time.deltaTime;
        
        //�ð��� 3�ʰ� ������ ���� �����ϱ�
        if(elapsedTime > 3f)
        {
            transform.Rotate(Vector3.back, Time.deltaTime*720f);
            turnTime += Time.deltaTime;
        }

        //�ѹ����� �������� ���� ���߱�
        if (turnTime >= 0.5f)
        {
            elapsedTime = 0;
            turnTime = 0;
        }

    }


    private void Update()
    {
        // �ð� ������Ʈ
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
