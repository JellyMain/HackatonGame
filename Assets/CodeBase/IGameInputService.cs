using UnityEngine;


public interface IGameInputService : IService
{
    public Vector2 GetNormalizedMovement();
}
