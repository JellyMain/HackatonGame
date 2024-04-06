using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    private IGameInputService gameInputService;
    private Rigidbody2D rb2d;


    private void Awake()
    {
        gameInputService = AllServices.Container.Single<IGameInputService>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        rb2d.velocity = gameInputService.GetNormalizedMovement() * moveSpeed;
    }
}