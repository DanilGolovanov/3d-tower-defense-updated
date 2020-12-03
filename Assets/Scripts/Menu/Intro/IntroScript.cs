﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public GameObject introPanel;
    public void AnimatorEventShowTitle()
    {
        introPanel.SetActive(true);
    }
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}