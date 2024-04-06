using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    private Game game;


    private void Awake()
    {
        game = new Game();
        DontDestroyOnLoad(this);
    }
    
}