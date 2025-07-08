using UnityEngine;

public class Bootstrap : MonoBehaviour
{
#if !UNITY_EDITOR
    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
#endif
}