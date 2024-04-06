using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private IGameInputService gameInputService;
    private FishBase fishBase;
    private Rigidbody2D rb2d;
    private float targetAngle;


    private void Awake()
    {
        gameInputService = AllServices.Container.Single<IGameInputService>();
        fishBase = GetComponent<FishBase>();
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

        rb2d.velocity = transform.right * fishBase.movementSpeed;

        if (moveInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        }

        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fishBase.rotationSpeed);
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
