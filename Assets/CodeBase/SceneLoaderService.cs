using UnityEngine.SceneManagement;


public class SceneLoaderService : ISceneLoaderService
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
