#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class GridSceneSetup : EditorWindow
{
    [MenuItem("Tools/Setup Grid Scene")]
    public static void SetupScene()
    {
        // Crear carpeta Resources si no existe
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/GridAssets"))
            AssetDatabase.CreateFolder("Assets/Resources", "GridAssets");

        // Copiar FBX de miniArena a Resources
        CopyToResources("Assets/Kenney/miniArena/FBX format/floor.fbx",          "Assets/Resources/GridAssets/floor.fbx");
        CopyToResources("Assets/Kenney/miniArena/FBX format/column.fbx",         "Assets/Resources/GridAssets/column.fbx");
        CopyToResources("Assets/Kenney/miniArena/FBX format/border-straight.fbx","Assets/Resources/GridAssets/border-straight.fbx");
        CopyToResources("Assets/Kenney/miniArena/FBX format/border-corner.fbx",  "Assets/Resources/GridAssets/border-corner.fbx");
        CopyToResources("Assets/Kenney/miniArena/FBX format/Textures/colormap.png","Assets/Resources/GridAssets/colormap.png");

        // Conectar cámara al Grid
        CameraController cam = Object.FindFirstObjectByType<CameraController>();
        GameObject grid = GameObject.Find("Grid");
        if (cam != null && grid != null)
        {
            cam.target = grid.transform;
            EditorUtility.SetDirty(cam);
            Debug.Log("✅ Cámara conectada.");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ Assets copiados a Resources. Ahora dale Play!");
    }

    static void CopyToResources(string src, string dst)
    {
        if (!File.Exists(dst))
        {
            AssetDatabase.CopyAsset(src, dst);
            Debug.Log($"✅ Copiado: {dst}");
        }
        else
        {
            Debug.Log($"Ya existe: {dst}");
        }
    }
}
#endif
