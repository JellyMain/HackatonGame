public class Game
{
    public Game()
    {
        RegisterServices();
        LoadGameScene();
    }


    private void RegisterServices()
    {
        AllServices.Container.RegisterSingle<IGameInputService>(new GameInputService());
        AllServices.Container.RegisterSingle<ISceneLoaderService>(new SceneLoaderService());
    }


    private void LoadGameScene()
    {
        AllServices.Container.Single<ISceneLoaderService>().LoadScene(1);
    }
}