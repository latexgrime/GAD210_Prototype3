using System;
using Unity.VisualScripting;
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
            // Get the reference to the CamTargetOrientation game object.
            camTargetOrientation = GameObject.FindGameObjectWithTag("Player").transform.Find("CamTargetOrientation");
        }
        
        public void MakeCursorVisible()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void MakeCursorInvisible()
        {
            Cursor.lockState = CursorLockMode.Locked;

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