using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class SceneManagement : MonoBehaviour
    {
        public void LoadSceneFromInt(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            LoadSceneFromInt(currentSceneIndex);
        }
    }
}