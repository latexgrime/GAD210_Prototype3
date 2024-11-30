using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu; 

    private bool isPaused = false;

    private void Start()
    {
        SetCursorLock(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); 
        Time.timeScale = 0f;       
        isPaused = true;
        SetCursorLock(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f;        
        isPaused = false;
        SetCursorLock(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;        
        SceneManager.LoadScene(0); 
    }

    public void SetCursorLock(bool locked)
    {
        if (locked == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if(locked == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
