using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private PlayerInput controls;
    private Vector3 dir;

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
        dir = new Vector3(direction.x, 0f, direction.y);
    }

    private void Stopping()
    {
        dir = Vector3.zero;
    }

    private void Update()
    {
        Move(dir);
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }
}
