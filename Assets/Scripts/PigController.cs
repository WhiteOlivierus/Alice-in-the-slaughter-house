using UnityEngine.InputSystem;
using UnityEngine;

public class PigController : MonoBehaviour
{
    private GameInput controls;

    private void Awake()
    {
        controls = new GameInput();
    }

    private void OnEnable()
    {
        controls.Player.Fire.performed += ctx => ShowCommands();
        controls.Player.Fire.Enable();
    }


    private void OnDisable()
    {
        controls.Player.Fire.performed -= ctx => ShowCommands();
        controls.Player.Fire.Disable();
    }

    private void ShowCommands() { }
}
