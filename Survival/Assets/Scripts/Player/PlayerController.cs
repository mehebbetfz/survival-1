using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpForce = 8f;
    public float gravity = 15f;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float mouseSensivity = 2f;
    public float xRotation = 0f;

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 1.3f;
    public LayerMask groundLayer;
    public float groundCheckOffset = 0.1f;


    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standHeight = 1f;
    public bool isCrouching = false;
    public Vector3 targetScale;

    [Header("Crouch Settings")]
    public float maxSlopeAngle = 45f;

    [Header("Other Settings")]
    private Vector3 velocity;
    public bool isGrounded;
    public bool isRunning = false;
    private Transform cameraHolder;

    private void Start()
    {
        targetScale = new Vector3(1, standHeight, 1);
        transform.localScale = targetScale;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReallyIsGrounded();
        HandleMovement();
        HandleJumping();
        HandleCrouch();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y -= gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    private void HandleJumping()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Start: " + velocity.y);
            velocity.y = Mathf.Sqrt(jumpForce * gravity);
            Debug.Log("End: " + velocity.y);

        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;

            targetScale.y = isCrouching ? crouchHeight : standHeight;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, crouchSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        // Homework
    }

    private void ReallyIsGrounded()
    {

        Vector3 rayOrigin = transform.position + (Vector3.up * groundCheckOffset);

        isGrounded = Physics.Raycast(rayOrigin, Vector3.down,
                                   groundCheckDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}
