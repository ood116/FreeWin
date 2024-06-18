using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor : EditorWindow
{
    [MenuItem("Editor_BW/PlayerPrefsClear")]
    static void PlayerPrefsClear() => PlayerPrefs.DeleteAll();
}
