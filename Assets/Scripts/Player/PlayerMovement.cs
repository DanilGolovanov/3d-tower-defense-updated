using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic player movement script, handles movement speed/gravity and jumping

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController; //using character controller mainly for grounded checks
    private Vector3 moveDirection;
    public float speed = 5f;
    private float gravity = 20f;

    public float jumpForce = 10f;
    private float vertialVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;
        applyGravity();
        characterController.Move(moveDirection);
    }
    void applyGravity()
    {
        if (characterController.isGrounded)
        {
            vertialVelocity -= gravity * Time.deltaTime;
            PlayerJump();
        }
        else
        {
            vertialVelocity -= gravity * Time.deltaTime;
        }
        moveDirection.y = vertialVelocity * Time.deltaTime;
    }
    void PlayerJump()
    {
        if(characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertialVelocity = jumpForce;
        }
    }
}
