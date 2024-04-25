using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public void GoBackButton()
    {
        GameManager.Instance.isPaused = false;
    }

    public void GoTitleButton()
    {
        GameManager.Instance.isPaused = false;
        SceneManager.LoadScene("Title");
    }
}
