using System;
using UnityEngine;

namespace GAD210.Leonardo.Player.CameraControl
{
    /// <summary>
    ///     This script is in charge on handling the Input for the camera and handling the camera itself.
    /// </summary>
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        private Transform camTargetOrientation;

        private float xRotation;
        private float yRotation;

        private void Start()
        {
            // Makes cursor invisible and locks in in the middle of the screen.
            InitializeCursor();

            // Get the reference to the CamTargetOrientation game object.
            camTargetOrientation = GameObject.FindGameObjectWithTag("Player").transform.Find("CamTargetOrientation");
        }

        private static void InitializeCursor()
        {
            // This locks the cursor in the middle of the screen.
            Cursor.lockState = CursorLockMode.Locked;
            // Make the cursor invisible.
            Cursor.visible = false;
        }

        private void Update()
        {
            // Handle player input.
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            // Handle camera rotation.
            yRotation += mouseX;
            xRotation -= mouseY;

            // Clamp rotation so player can't spin around like crazy.
            xRotation = Math.Clamp(xRotation, -90f, 90f);

            // Rotate camera and the cameraOrientation object.
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            camTargetOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}