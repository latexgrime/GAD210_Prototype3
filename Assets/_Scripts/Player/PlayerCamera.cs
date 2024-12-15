using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GAD210.Leonardo.Player.CameraControl
{
    /// <summary>
    ///     This script is in charge on handling the Input for the camera and handling the camera itself.
    /// </summary>
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private float sensitivitySliderValue;

        private Transform camTargetOrientation;
        private float xRotation;
        private float yRotation;

        private void Start()
        {
            // Get the reference to the CamTargetOrientation game object.
            camTargetOrientation = GameObject.FindGameObjectWithTag("Player").transform.Find("CamTargetOrientation");

            if (sensitivitySlider != null)
            {
                sensitivitySlider.value = 100;
                sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
                UpdateSensitivity(sensitivitySlider.value);
            }
        }

        private void UpdateSensitivity(float value)
        {
            sensitivitySliderValue = value;
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
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivitySliderValue;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivitySliderValue;

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