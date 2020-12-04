using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //control bool
    public static bool GameIsPaused = false;
    //reference to canvas
    public GameObject PauseMenuUI;

    private void Start()
    {
        Resume();
    }
    //mapping escape to key to open/close pause menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    /// <summary>
    /// method to turn off pause screen by disabling canvas
    /// unlocks time and cursor
    /// </summary>
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    /// <summary>
    /// method to turn on pause screen by enabling canvas
    /// freezes time and cursor 
    /// </summary>
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
}
