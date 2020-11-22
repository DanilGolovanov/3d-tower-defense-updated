using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    //reference to gameobjects
    public GameObject pressAnyKeyPanel, menuPanel;
    //variable for controlling input function
    private int control;

    void Start()
    {
        menuPanel.SetActive(false);
        control = 0;
    }
    //if menupanel is off, set menupanel active
    //set press any ken panel inactive
    //toggle control variable
    void Update()
    {
        if (control == 0)
        {
            if (Input.anyKey)
            {
                menuPanel.SetActive(true);
                pressAnyKeyPanel.SetActive(false);
                control = 1;
            }
        }

    }
}
