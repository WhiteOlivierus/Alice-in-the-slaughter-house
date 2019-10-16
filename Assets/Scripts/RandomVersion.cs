using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomVersion : MonoBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadRandomScene()
    {
        int version = Random.Range(1, 4);
        SceneManager.LoadScene("V" + version);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }
}
