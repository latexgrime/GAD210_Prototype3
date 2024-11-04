using UnityEngine;

namespace GAD210.Leonardo.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] public float moveSpeed;

        public float groundDrag;

        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        private bool readyToJump;

        [HideInInspector] public float walkSpeed = 7f;
        [HideInInspector] public float sprintSpeed = 14f;

        [Header("Keybindings")] public KeyCode jumpKey = KeyCode.Space;

        [Header("Ground Check")] public float playerHeight;
        public LayerMask whatIsGround;
        private bool grounded;

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
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

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