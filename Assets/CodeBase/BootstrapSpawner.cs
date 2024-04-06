using UnityEngine;


public class BootstrapSpawner : MonoBehaviour
{
    public Bootstrap gameBootstrapperPrefab;
    
    private void Awake()
    {
        Bootstrap gameBootstrapper = FindObjectOfType<Bootstrap>();

        if (gameBootstrapper == null)
        {
            Instantiate(gameBootstrapperPrefab);
        }
    }
}
