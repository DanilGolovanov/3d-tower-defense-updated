using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//script to handle gameover menu
//adds public methods to add to onclick events for UI buttons

public class GameOver : MonoBehaviour
{
    //exit method
    //if in editor, exit
    //otherwise exit application
    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    //load scene by string
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
