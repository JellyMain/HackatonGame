using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;
    
    private IGameInputService gameInputService;
    private Rigidbody2D rb2d;
    private float rotationAngle;

    private void Awake()
    {
        gameInputService = AllServices.Container.Single<IGameInputService>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        Move();
    }

    
    private void LateUpdate()
    {
        FlipSprite();
    }

    
    private void Move()
    {
        Vector2 moveInput = gameInputService.GetNormalizedMovement();
        
        rb2d.velocity = transform.right * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            rotationAngle += moveInput.y * rotationSpeed;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        }
    }
    
    

    private void FlipSprite()
    {
        if (rb2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
