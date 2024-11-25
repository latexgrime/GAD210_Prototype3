using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAD210.Leonardo.Player.Movement
{
    /// <summary>
    ///     Script in charge of handling the input for the movement of the player.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] 
        private float moveSpeed;
        public float groundDrag;
        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        private bool readyToJump;
        private bool isRunning = false;

        [SerializeField] private float walkSpeed = 7f;
        [SerializeField] private float sprintSpeed = 14f;

        [Header("Keybindings")] 
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Ground Check")] 
        public float playerHeight;
        public LayerMask whatIsGround;
        [SerializeField] private bool grounded;
        private bool wasGrounded;
        public Transform orientation;

        [Header("SFX")] 
        private AudioSource audioSource;

        [SerializeField] private AudioClip walkInSandSFX;
        [SerializeField] private AudioClip walkInWaterSFX;
        [SerializeField] private AudioClip walkInWoodSFX;
        [SerializeField] private AudioClip walkInRockSFX;
        [SerializeField] private AudioClip landSFX;
        [SerializeField] private AudioClip jumpSFX;
        
        /// <summary>
        /// The amount of time that needs to happen before a step sound can be played again.
        /// </summary>
        [SerializeField] private float stepIntervalForWalkSFX;
        [SerializeField] private float stepIntervalForRunSFX;
        private float stepTimer;
            
        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDirection;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            audioSource = GetComponent<AudioSource>();

            readyToJump = true;
        }

        private void Update()
        {
            // Ground check.
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 1.1f, whatIsGround);
                
            PlayerInput();
            SpeedControl();

            // Handle drag.
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            wasGrounded = grounded;
        }

        private void FixedUpdate()
        {
            MovePlayer();
            
            // Play the landing sfx once.
            if (grounded && !wasGrounded)
            {
                audioSource.PlayOneShot(landSFX);
            }
        }

        private void PlayerInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // Can jump.
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
            
            if (Input.GetKey(sprintKey))
            {
                moveSpeed = sprintSpeed;
                isRunning = true;
            }
            else
            {
                moveSpeed = walkSpeed;
                isRunning = false;
            }

            PlayWalkingSFX();
        }

        
        private void PlayWalkingSFX()
        {
            bool isMoving = horizontalInput != 0 || verticalInput != 0;
            if (grounded && isMoving)
            {
                stepTimer += Time.deltaTime;
                
                float currentStepIntervalSFX;
                if (isRunning)
                {
                    currentStepIntervalSFX = stepIntervalForRunSFX;
                }
                else
                {
                    currentStepIntervalSFX = stepIntervalForWalkSFX;
                }
                
                if (stepTimer >= currentStepIntervalSFX)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.volume = Random.Range(0.8f, 1.2f);

                    AudioClip surfaceSFX = GetSurfaceStepSFX();
                    audioSource.PlayOneShot(surfaceSFX);
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = 0f;
                audioSource.pitch = 1;
                audioSource.volume = 1;
            }
        }

        private AudioClip GetSurfaceStepSFX()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 1.1f, whatIsGround))
            {
                string surfaceTag = hit.transform.tag;

                switch (surfaceTag)
                {
                    case "Sand":
                        return walkInSandSFX;
                    case "Wood":
                        return walkInWoodSFX;
                    case "Water":
                        return walkInWaterSFX;
                    case "Rock":
                        return walkInRockSFX;
                }
            }

            // If there is nothing just default to sand lol.
            return walkInSandSFX;
        }

        private void MovePlayer()
        {
            // Calculate the movement direction.
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // Is in ground.
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            // Is in air.
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        private void SpeedControl()
        {
            var flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit velocity if needed.
            if (flatVel.magnitude > moveSpeed)
            {
                var limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        private void Jump()
        {
            // Reset Y velocity.
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            audioSource.PlayOneShot(jumpSFX);
        }

        private void ResetJump()
        {
            audioSource.PlayOneShot(landSFX);
            readyToJump = true;
        }
    }
}