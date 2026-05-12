using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Setup automatico de Level1. Se autodestruye tras configurar la escena.
/// </summary>
public class Level1Setup : MonoBehaviour
{
    void Awake()
    {
        // Configura el tag del player
        GameObject player = GameObject.Find("Player");
        if (player != null) player.tag = "Player";

        // Configura colliders de obstaculos como solidos
        string[] obstacles = {"Obstacle_2_1","Obstacle_3_1","Obstacle_1_3","Obstacle_4_3",
                               "Obstacle_2_5","Obstacle_5_2","Obstacle_3_4","Obstacle_6_3"};
        foreach (var name in obstacles)
        {
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                var bc = obj.GetComponent<BoxCollider>() ?? obj.AddComponent<BoxCollider>();
                bc.isTrigger = false;
            }
        }

        // Configura la meta con trigger
        var goal = GameObject.Find("Goal");
        if (goal != null)
        {
            var cc = goal.GetComponent<CapsuleCollider>();
            if (cc != null) cc.isTrigger = true;
            var lg = goal.GetComponent<LevelGoal>() ?? goal.AddComponent<LevelGoal>();
            lg.levelIndex = 1;
        }

        // Posiciona el player en celda (0,0)
        if (player != null)
            player.transform.position = new Vector3(-7f, 0.5f, -7f);

        // Posiciona la camara detras del player
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(-7f, 12f, -21f);
            cam.transform.rotation = Quaternion.Euler(45, 0, 0);
        }

        Destroy(this); // no necesario despues del primer frame
    }
}
