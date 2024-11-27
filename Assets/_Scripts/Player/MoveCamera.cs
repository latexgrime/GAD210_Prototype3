using UnityEngine;

namespace GAD210.Leonardo.Player.CameraMovement
{
    /// <summary>
    ///     This scripts just sets the position of the camera holder to the cameraTargetPosition.
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        private Transform cameraTargetPosition;

        private void Start()
        {
            // Get the reference to the CamTargetPosition game object.
            cameraTargetPosition = GameObject.FindGameObjectWithTag("Player").transform.Find("CamTargetPosition");
        }

        private void Update()
        {
            transform.position = cameraTargetPosition.position;
        }
    }
}