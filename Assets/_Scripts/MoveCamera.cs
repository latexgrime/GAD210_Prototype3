using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAD210.Leonardo.Player.CameraMovement
{
    /// <summary>
    /// This scripts just sets the position of the camera holder to the cameraTargetPosition.
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraTargetPosition;

        private void Update()
        {
            transform.position = cameraTargetPosition.position;
        }
    }
}