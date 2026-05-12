using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// Script de Editor: configura automaticamente todas las escenas del juego.
/// Ejecutar desde Tools > Setup All Levels
/// </summary>
#if UNITY_EDITOR
public class LevelSetupEditor : EditorWindow
{
    [MenuItem("Tools/Setup All Levels")]
    public static void SetupAllLevels()
    {
        // Agregar todas las escenas al Build Settings
        AddScenesToBuild();
        Debug.Log("✅ Todas las escenas agregadas al Build Settings.");
        EditorUtility.DisplayDialog("Setup Completo",
            "Escenas creadas y agregadas al Build Settings.\n\nAbre cada escena manualmente y asigna los prefabs Kenney en el Inspector del LevelBuilder.",
            "OK");
    }

    static void AddScenesToBuild()
    {
        var scenes = new[]
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Level1.unity",
            "Assets/Scenes/Level2.unity",
            "Assets/Scenes/Level3.unity",
            "Assets/Scenes/Level4.unity",
            "Assets/Scenes/Level5.unity",
            "Assets/Scenes/Level6.unity",
            "Assets/Scenes/Level7.unity",
            "Assets/Scenes/WinScreen.unity"
        };

        var list = new System.Collections.Generic.List<EditorBuildSettingsScene>();
        foreach (var path in scenes)
            list.Add(new EditorBuildSettingsScene(path, true));

        EditorBuildSettings.scenes = list.ToArray();
    }
}
#endif
