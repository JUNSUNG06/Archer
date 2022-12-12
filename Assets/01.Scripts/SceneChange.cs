using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void DieScene()
    {
        SceneManager.LoadScene("Die");
    }

    public void ClearScene()
    {
        SceneManager.LoadScene("Clear");
    }
}
