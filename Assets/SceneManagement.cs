using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public static class SceneManagement
{
    public static void LoadSceneFromInt(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadSceneFromInt(currentSceneIndex);
    }
}