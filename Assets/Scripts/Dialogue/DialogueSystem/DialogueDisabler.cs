#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DialogueDisabler : MonoBehaviour
{
    private const string DISABLE_DIALOGUE_MENU = "Custom Tools/Disable dialogue";
    private static bool _disableDialogue = false;

    static DialogueDisabler()
    {
        _disableDialogue = EditorPrefs.GetBool(DISABLE_DIALOGUE_MENU, false);

        EditorApplication.delayCall += () => {
            ToggleForceBootstrap(_disableDialogue);
        };
    }

    public static bool GetIsDialogueDisabled() { return _disableDialogue; }

    [MenuItem(DISABLE_DIALOGUE_MENU)]
    private static void HandleForceBootstrapMenuItem()
    {
        ToggleForceBootstrap(!_disableDialogue);
    }

    private static void ToggleForceBootstrap(bool state)
    {
        _disableDialogue = state;
        Menu.SetChecked(DISABLE_DIALOGUE_MENU, state);
        EditorPrefs.SetBool(DISABLE_DIALOGUE_MENU, state);
    }
}
#endif