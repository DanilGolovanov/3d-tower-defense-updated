using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script to disable cursor locking
public class OutroManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
