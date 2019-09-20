using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player movement controller
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //Public variables through blackboard
    private float Speed = BlackBoard.PlayerSpeed;

    private GameInput controls;
    private Vector3 direction;

    private void Awake()
    {
        controls = new GameInput();
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => SetDirection(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => ResetDirection();
        controls.Player.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= ctx => SetDirection(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled -= ctx => ResetDirection();
        controls.Player.Move.Disable();
    }

    private void Update()
    {
        Move(this.direction);
    }

    /// <summary>
    /// Set the input direction to move
    /// </summary>
    /// <param name="direction"></param>
    private void SetDirection(Vector2 direction)
    {
        this.direction = new Vector3(direction.x, 0f, direction.y);
    }

    /// <summary>
    /// Reset the input direction to zero
    /// </summary>
    private void ResetDirection()
    {
        this.direction = Vector3.zero;
    }

    /// <summary>
    /// Move the player towards the input direction
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector3 direction)
    {
        transform.position += direction * Speed * Time.fixedDeltaTime;
    }
}
