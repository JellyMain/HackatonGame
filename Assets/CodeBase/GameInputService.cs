using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameInputService : IGameInputService
{
    private readonly InputActions inputActions;


    public GameInputService()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }


    public Vector2 GetNormalizedMovement()
    {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}