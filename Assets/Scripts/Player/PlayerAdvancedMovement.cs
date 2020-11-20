using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic script to handle crouching and sprinting

public class PlayerAdvancedMovement : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private Transform lookRoot;
    private float standingHeight = 1.6f;
    private float crouchingHeight = 1f;
    private bool isCrouching;

    public GameObject arms;
    Animator anim;

    private PlayerAudio playerAudio;
    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;
    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f;
    private float crouch_Step_Distance = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        anim = arms.GetComponent<Animator>();
        playerAudio = GetComponentInChildren<PlayerAudio>();//player audio is child of player
        lookRoot = transform.GetChild(0); //look root is child of player 
    }

    private void Start()
    {
        playerAudio.volume_Min = walk_Volume_Min;
        playerAudio.volume_Max = walk_Volume_Max;
        playerAudio.step_Distance = walk_Step_Distance;
    }

    void Update()
    {
        Sprint();
        Crouch();
        tempWalkAnimation();
    }

    //will circle back here a later stage to add stamina 
    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = sprintSpeed;
            anim.SetBool("isSprinting", true);
            playerAudio.step_Distance = sprint_Step_Distance;
            playerAudio.volume_Min = sprint_Volume;
            playerAudio.volume_Max = sprint_Volume;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;
            anim.SetBool("isSprinting", false);
            playerAudio.step_Distance = walk_Step_Distance;
            playerAudio.volume_Min = walk_Volume_Min;
            playerAudio.volume_Max = walk_Volume_Max;
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                lookRoot.localPosition = new Vector3(0f, standingHeight, 0f); //default standing height is 
                playerMovement.speed = moveSpeed;
                playerAudio.step_Distance = walk_Step_Distance;
                playerAudio.volume_Min = walk_Volume_Min;
                playerAudio.volume_Max = walk_Volume_Max;

                isCrouching = false;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0f, crouchingHeight, 0f);
                playerMovement.speed = crouchSpeed;
                playerAudio.step_Distance = crouch_Step_Distance;
                playerAudio.volume_Min = crouch_Volume;
                playerAudio.volume_Max = crouch_Volume;

                isCrouching = true;
            }
        }
    }
    void tempWalkAnimation()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}
