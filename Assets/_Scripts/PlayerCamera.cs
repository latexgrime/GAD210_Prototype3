using System;
using UnityEngine;

namespace GAD210.Leonardo.Player.CameraControl
{
    public class PlayerCamera : MonoBehaviour
    {
        public float sensitivityX;
        public float sensitivityY;

        public Transform cameraOrientation;

        private float xRotation;
        private float yRotation;

        private void Start()
        {
            // Makes cursor invisible and locks in in the middle of the screen.
            InitializeCursor();
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
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            // Handle camera rotation.
            yRotation += mouseX;
            xRotation -= mouseY;
            
            // Clamp rotation so player can't spin around like crazy.
            xRotation = Math.Clamp(xRotation, -90f, 90f);
            
            transform.rotation = Quaternion.Euler(xRotation, yRotation,0);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}