using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    private PlayerInput controls;
    private Vector3 direction;

    private void Awake()
    {
        controls = new PlayerInput();
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => Moving(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => Stopping();
        controls.Player.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= ctx => Moving(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled -= ctx => Stopping();
        controls.Player.Move.Disable();
    }

    private void Moving(Vector2 direction)
    {
        this.direction = new Vector3(direction.x, 0f, direction.y);
    }

    private void Stopping()
    {
        this.direction = Vector3.zero;
    }

    private void Update()
    {
        Move(this.direction);
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * Speed * Time.fixedDeltaTime;
    }
}
