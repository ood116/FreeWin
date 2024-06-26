using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenSceneEditor : EditorWindow
{
    [MenuItem("Editor_BW/OpenScene/1. Login")]
    static void Login() => SceneOpen(0);

    [MenuItem("Editor_BW/OpenScene/2. Lobby")]
    static void Lobby() => SceneOpen(1);

    [MenuItem("Editor_BW/OpenScene/3. Session")]
    static void Session() => SceneOpen(2);

    [MenuItem("Editor_BW/OpenScene/4. UITemp")]
    static void UITemp() => SceneOpen(3);

    static public void SceneOpen(int SceneIndex)
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[SceneIndex].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        var sceneName = sceneAsset.ToString().Split(' ');

        if (sceneAsset != null) {
            EditorSceneManager.OpenScene("Assets/Association/_Scene/" + sceneName[0] + ".unity");
        }
    }
}
