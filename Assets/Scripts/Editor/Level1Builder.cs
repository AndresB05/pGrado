using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class Level1Builder : EditorWindow
{
    [MenuItem("Tools/Build Level1 Grid")]
    public static void BuildLevel1()
    {
        var oldGrid=GameObject.Find("_KenneyGrid"); if(oldGrid!=null) Object.DestroyImmediate(oldGrid);

        GameObject floorFBX=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GridAssets/floor.fbx");
        GameObject colFBX=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GridAssets/column.fbx");
        GameObject borderS=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GridAssets/border-straight.fbx");
        GameObject borderC=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GridAssets/border-corner.fbx");
        GameObject soldier=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Kenney/miniArena/FBX format/character-soldier.fbx");
        GameObject trophy=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Kenney/miniArena/FBX format/trophy.fbx");
        if(floorFBX==null){Debug.LogError("floor.fbx no encontrado");return;}

        Shader sh=Shader.Find("Universal Render Pipeline/Lit");if(sh==null)sh=Shader.Find("Standard");
        Material mat=new Material(sh);
        Texture2D tex=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/GridAssets/colormap.png");
        if(tex!=null)mat.mainTexture=tex;

        int W=4,H=4;float cs=2f;
        GameObject root=new GameObject("_KenneyGrid");

        // SUELO
        for(int x=0;x<W;x++)for(int z=0;z<H;z++){
            float px=x*cs-(W-1)*cs/2f,pz=z*cs-(H-1)*cs/2f;
            var t=(GameObject)PrefabUtility.InstantiatePrefab(floorFBX);
            t.name="F_"+x+"_"+z;t.transform.position=new Vector3(px,0,pz);
            t.transform.localScale=Vector3.one*1.85f;t.transform.SetParent(root.transform);
            foreach(var r in t.GetComponentsInChildren<Renderer>(true))r.sharedMaterial=mat;
        }

        // OBSTACULOS - posiciones que NO bloqueen al jugador en (0,0)
        // Camino libre: (0,0)->(1,0)->(2,0)->(3,0)->(3,1)->(3,2)->(3,3)
        var obsRoot=new GameObject("_Obs");obsRoot.transform.SetParent(root.transform);
        int[][]obs=new int[][]{ new[]{0,1},new[]{1,1},new[]{0,2},new[]{1,2} };
        foreach(var o in obs){
            float px=o[0]*cs-(W-1)*cs/2f,pz=o[1]*cs-(H-1)*cs/2f;
            if(colFBX==null){Debug.LogWarning("column.fbx no encontrado");continue;}
            var col=(GameObject)PrefabUtility.InstantiatePrefab(colFBX);
            col.name="Obs_"+o[0]+"_"+o[1];
            col.transform.position=new Vector3(px,0,pz);
            col.transform.localScale=Vector3.one*1.4f;
            col.transform.SetParent(obsRoot.transform);
            foreach(var r in col.GetComponentsInChildren<Renderer>(true))r.sharedMaterial=mat;
            // Collider invisible encima del FBX para bloquear paso
            var blocker=new GameObject("Collider");
            blocker.transform.SetParent(col.transform);
            blocker.transform.localPosition=new Vector3(0,0.5f,0);
            var bc=blocker.AddComponent<BoxCollider>();
            bc.size=new Vector3(0.9f,2f,0.9f);
            bc.isTrigger=false;
        }

        // BORDES
        if(borderS!=null&&borderC!=null){
            var bRoot=new GameObject("_Borders");bRoot.transform.SetParent(root.transform);
            float half=(W-1)*cs/2f,off=cs*0.6f;
            for(int i=0;i<W;i++){float t2=i*cs-half;
                SpawnB(borderS,new Vector3(t2,0,half+off),0,bRoot,mat);
                SpawnB(borderS,new Vector3(t2,0,-half-off),180,bRoot,mat);
                SpawnB(borderS,new Vector3(half+off,0,t2),90,bRoot,mat);
                SpawnB(borderS,new Vector3(-half-off,0,t2),270,bRoot,mat);
            }
            float co=half+off;
            SpawnB(borderC,new Vector3(co,0,co),0,bRoot,mat);
            SpawnB(borderC,new Vector3(-co,0,co),90,bRoot,mat);
            SpawnB(borderC,new Vector3(-co,0,-co),180,bRoot,mat);
            SpawnB(borderC,new Vector3(co,0,-co),270,bRoot,mat);
        }

        // META - trophy en celda (3,3)
        float gx=3*cs-(W-1)*cs/2f,gz=3*cs-(H-1)*cs/2f;
        if(trophy!=null){
            var g=(GameObject)PrefabUtility.InstantiatePrefab(trophy);
            g.name="GoalTrophy";g.transform.position=new Vector3(gx,0,gz);
            g.transform.localScale=Vector3.one*2f;g.transform.SetParent(root.transform);
            foreach(var r in g.GetComponentsInChildren<Renderer>(true))r.sharedMaterial=mat;
        }

        // Goal trigger
        var goal=GameObject.Find("Goal");if(goal==null)goal=new GameObject("Goal");
        goal.transform.position=new Vector3(gx,0.5f,gz);
        foreach(var c in goal.GetComponents<Collider>())Object.DestroyImmediate(c);
        var goalBC=goal.AddComponent<BoxCollider>();
        goalBC.isTrigger=true;goalBC.size=new Vector3(1.8f,2f,1.8f);goalBC.center=new Vector3(0,1f,0);
        var lg=goal.GetComponent<LevelGoal>()??goal.AddComponent<LevelGoal>();
        lg.levelIndex=1;

        // PLAYER
        var p=GameObject.Find("Player");if(p==null)p=new GameObject("Player");
        foreach(Transform c in p.transform)Object.DestroyImmediate(c.gameObject);
        foreach(var c in p.GetComponents<MeshFilter>())Object.DestroyImmediate(c);
        foreach(var c in p.GetComponents<MeshRenderer>())Object.DestroyImmediate(c);
        foreach(var c in p.GetComponents<CapsuleCollider>())Object.DestroyImmediate(c);
        if(soldier!=null){
            var s=(GameObject)PrefabUtility.InstantiatePrefab(soldier);
            s.name="SoldierModel";s.transform.SetParent(p.transform);
            s.transform.localPosition=Vector3.zero;
            s.transform.localRotation=Quaternion.identity;
            s.transform.localScale=Vector3.one*3.5f;
            foreach(var r in s.GetComponentsInChildren<Renderer>(true))r.sharedMaterial=mat;
        }
        p.transform.position=new Vector3(-(W-1)*cs/2f,0.05f,-(H-1)*cs/2f);
        p.tag="Player";
        var pc=p.GetComponent<PlayerController>()??p.AddComponent<PlayerController>();
        pc.gridWidth=W;pc.gridHeight=H;pc.cellSize=cs;pc.startCell=new Vector2Int(0,0);
        var pcc=p.AddComponent<CapsuleCollider>();
        pcc.height=1.8f;pcc.radius=0.35f;pcc.center=new Vector3(0,0.9f,0);pcc.isTrigger=false;

        // CAMARA
        GameObject camGO=GameObject.Find("Main Camera");
        if(camGO!=null){
            camGO.transform.position=new Vector3(0,10,-12);
            camGO.transform.rotation=Quaternion.Euler(40,0,0);
            var cf=camGO.GetComponent<CameraFollow>()??camGO.AddComponent<CameraFollow>();
            cf.target=p.transform;cf.offset=new Vector3(0,8,-10);cf.topDownOffset=new Vector3(0,18,0);
        }

        // GAMEMANAGER
        var gm=GameObject.Find("GameManager");if(gm==null)gm=new GameObject("GameManager");
        if(gm.GetComponent<GameManager>()==null)gm.AddComponent<GameManager>();

        // LUZ
        var dl=GameObject.Find("Directional Light");
        if(dl!=null){var l=dl.GetComponent<Light>();
            if(l!=null){l.color=new Color(1f,0.95f,0.8f);l.intensity=1.2f;l.shadows=LightShadows.Soft;}
            dl.transform.rotation=Quaternion.Euler(50,-30,0);}

        // HUD
        LevelHUDBuilder.CreateHUD(1);

        // Limpiar objetos viejos
        foreach(var n in new[]{"Floor_Base","Obstacle_2_1","Obstacle_3_1","Obstacle_1_3","Obstacle_4_3","LevelBuilder"})
        { var ob=GameObject.Find(n); if(ob!=null)Object.DestroyImmediate(ob); }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorUtility.DisplayDialog("Level1 Listo","Listo! Guarda con Ctrl+S.","OK");
    }

    public static void SpawnB(GameObject pf,Vector3 pos,float ry,GameObject par,Material mat){
        var b=(GameObject)PrefabUtility.InstantiatePrefab(pf);
        b.transform.position=pos;b.transform.rotation=Quaternion.Euler(0,ry,0);
        b.transform.localScale=Vector3.one*1.85f;b.transform.SetParent(par.transform);
        foreach(var r in b.GetComponentsInChildren<Renderer>(true))r.sharedMaterial=mat;
    }
}
#endif