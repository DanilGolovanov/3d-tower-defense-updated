using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic script to handle crouching and sprinting control, animations for movement & stamina depletion/regeneration. 

public class PlayerAdvancedMovement : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [Header ("Player Advanced Movement Calibration")]
    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;
    private Transform lookRoot;
    private float standingHeight = 1.6f;
    private float crouchingHeight = 1f;
    private bool isCrouching;
    public float sprintTreshold = 10f;
    //references
    public GameObject arms;
    Animator anim;
    //player audio settings for footsteps
    private PlayerAudio playerAudio;
    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f, walk_Volume_Max = 0.6f;
    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.5f;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        anim = arms.GetComponent<Animator>();
        playerAudio = GetComponentInChildren<PlayerAudio>();//player audio is child of player
        lookRoot = transform.GetChild(0); //look root is child of player 
    }

    private void Start()
    {
        playerAudio.volume_Min = walkVolumeMin;
        playerAudio.volume_Max = walk_Volume_Max;
        playerAudio.step_Distance = walkStepDistance;
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Sprint();
            Crouch();
            tempWalkAnimation();
        }
    }
    //stamina check
    //input check
    //change speed, animation and play audio based on distance moving
    void Sprint()
    {
        if(GameManager.currentStamina > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
            {
                playerMovement.speed = sprintSpeed;
                anim.SetBool("isSprinting", true);
                playerAudio.step_Distance = sprintStepDistance;
                playerAudio.volume_Min = sprintVolume;
                playerAudio.volume_Max = sprintVolume;

            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;
            anim.SetBool("isSprinting", false);
            playerAudio.step_Distance = walkStepDistance;
            playerAudio.volume_Min = walkVolumeMin;
            playerAudio.volume_Max = walk_Volume_Max;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            GameManager.currentStamina -= sprintTreshold * Time.deltaTime;

            if (GameManager.currentStamina <= 0f)
            {

                GameManager.currentStamina = 0f;

                // reset the speed and sound
                playerMovement.speed = moveSpeed;
                playerAudio.step_Distance = walkStepDistance;
                playerAudio.volume_Min = walkVolumeMin;
                playerAudio.volume_Max = walk_Volume_Max;
            }
        }
        else //regen stamina
        {
            if (GameManager.currentStamina != GameManager.maxStamina)
            {
                GameManager.currentStamina += (sprintTreshold / 2) * Time.deltaTime;
            }
            if (GameManager.currentStamina > GameManager.maxStamina)
            {
                GameManager.currentStamina = GameManager.maxStamina;
            }
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
                playerAudio.step_Distance = walkStepDistance;
                playerAudio.volume_Min = walkVolumeMin;
                playerAudio.volume_Max = walk_Volume_Max;

                isCrouching = false;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0f, crouchingHeight, 0f);
                playerMovement.speed = crouchSpeed;
                playerAudio.step_Distance = crouchStepDistance;
                playerAudio.volume_Min = crouchVolume;
                playerAudio.volume_Max = crouchVolume;

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
