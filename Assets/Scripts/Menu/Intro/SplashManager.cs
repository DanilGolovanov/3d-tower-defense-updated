using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //import scene manager namespace

public class SplashManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Coroutine()); // call method to wait for 5 seconds while splash intro movie plays
    }

    IEnumerator Coroutine() //IEnumerator method to wait for the intro movie to play before calling the mainmenu scene via index
    {
        //yield on a new YieldInstruction that waits for 4 seconds -- enough time for splash logo to play
        yield return new WaitForSeconds(4); //because it's not a void method we need to return 

        SceneManager.LoadScene(1); //load scene via scenemanager using index
    }
}
