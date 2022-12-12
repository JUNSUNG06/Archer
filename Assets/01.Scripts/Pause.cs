using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isOpen = false;

    private void Update()
    {
        SetPausePanel();
    }

    private void SetPausePanel()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pause");
            if(!isOpen)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                
                isOpen = true;
                Time.timeScale = 0f;
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }

                isOpen = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void Continue()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        isOpen = false;
        Time.timeScale = 1f;
    }
}
