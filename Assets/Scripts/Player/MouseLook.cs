using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header ("Mouse Look Calibration")]
    [SerializeField]
    private Transform playerRoot, lookRoot; //remember look root transform is different to player transform.
    [SerializeField]
    private bool invert; //if we will have invert in options panel for FPS controls
    [SerializeField]
    private bool canUnlock = true;
    [SerializeField]
    private float sensitivity = 5f;

    private Vector2 defaultLookLimit = new Vector2(-70f, 80f);
    private Vector2 lookAngles;
    private Vector2 currentMouseLook;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LockandUnlockCursor();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            lookAround();
        }
    }
    void LockandUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    void lookAround()
    {
        currentMouseLook = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f); //ternary operator checks if invert is true, use positive otherwise use negative float
        lookAngles.y += currentMouseLook.y * sensitivity; //no invert required on y axis
        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimit.x, defaultLookLimit.y); //dont allow values outside defaults by clamping

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f); //x axis only
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f); //y axis only
    }
}
