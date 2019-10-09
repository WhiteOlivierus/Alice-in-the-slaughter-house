using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomVersion : MonoBehaviour
{
    public void LoadRandomScene()
    {
        int version = Random.Range(1, 4);
        SceneManager.LoadScene("V" + version);
    }
}
