using GAD210.Leonardo.Player.CameraControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Game_scenes
{
    public class RestartEndingButton : MonoBehaviour
    {
        private PlayerCamera _playerCamera;

        private void Start()
        {
            MakeCursorVisible();
        }

        public void Restart()
        {
            SceneManager.LoadScene("Main_Menu");
        }

        public void MakeCursorVisible()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}