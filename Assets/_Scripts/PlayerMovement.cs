using Unity.VisualScripting;
using UnityEngine;

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

        [SerializeField] private float walkSpeed = 7f;
        [SerializeField] private float sprintSpeed = 14f;

        [Header("Keybindings")] 
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Ground Check")] 
        public float playerHeight;
        public LayerMask whatIsGround;
        [SerializeField] private bool grounded;

        public Transform orientation;

        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDirection;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyToJump = true;
        }

        private void Update()
        {
            // Ground check.
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, whatIsGround);

            MyInput();
            SpeedControl();

            // Handle drag.
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
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
            }
            else
            {
                moveSpeed = walkSpeed;
            }
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
        }

        private void ResetJump()
        {
            readyToJump = true;
        }
    }
}