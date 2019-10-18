using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersionUI : MonoBehaviour
{
    private GameInput controls;

    private void Awake()
    {
        controls = new GameInput();
        string v = SceneManager.GetActiveScene().name;
        GetComponent<TextMeshProUGUI>().text = v;
    }
    private void OnEnable()
    {
        controls.Player.ToMain.performed += ctx => GoToMain();
        controls.Player.ToMain.Enable();
    }

    private void OnDisable()
    {
        controls.Player.ToMain.performed += ctx => GoToMain();
        controls.Player.ToMain.Disable();
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("main");
    }
}
