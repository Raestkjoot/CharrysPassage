#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    private static void LoadDefaultScene(PlayModeStateChange state)
    {
        if (!EditorBootstrapMenuItems.ForceBootstrap())
            return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            if (SceneManager.GetActiveScene().name != "Bootstrap")
                SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
        }
    }
}

[InitializeOnLoad]
public class EditorBootstrapMenuItems : MonoBehaviour
{
    private const string FORCE_BOOTSTRAP_MENU = "Custom Tools/Force Bootstrap Scene";
    private static bool _forceBootstrap = true;

    static EditorBootstrapMenuItems()
    {
        _forceBootstrap = EditorPrefs.GetBool(FORCE_BOOTSTRAP_MENU, true);

        EditorApplication.delayCall += () => {
            ToggleForceBootstrap(_forceBootstrap);
        };
    }

    public static bool ForceBootstrap() { return _forceBootstrap; }

    [MenuItem(FORCE_BOOTSTRAP_MENU)]
    private static void HandleForceBootstrapMenuItem()
    {
        ToggleForceBootstrap(!_forceBootstrap);
    }

    private static void ToggleForceBootstrap(bool state)
    {
        _forceBootstrap = state;
        Menu.SetChecked(FORCE_BOOTSTRAP_MENU, state);
        EditorPrefs.SetBool(FORCE_BOOTSTRAP_MENU, state);
    }
}
#endif
