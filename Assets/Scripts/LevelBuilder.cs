using UnityEngine;

/// <summary>
/// Generador procedural de niveles con grid expandido.
/// Usa assets Kenney segun el tema del nivel.
/// </summary>
public class LevelBuilder : MonoBehaviour
{
    [Header("Grid")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 2f;

    [Header("Prefabs - Suelo")]
    public GameObject floorPrefab;

    [Header("Prefabs - Obstaculos")]
    public GameObject[] obstaclePrefabs;

    [Header("Prefabs - Borde")]
    public GameObject borderStraightPrefab;
    public GameObject borderCornerPrefab;

    [Header("Prefabs - Props decorativos")]
    public GameObject[] propPrefabs;

    [Header("Meta")]
    public GameObject goalPrefab;
    public Vector2Int goalCell = new Vector2Int(9, 9);

    [Header("Material")]
    public Material floorMaterial;
    public Material obstacleMaterial;

    [Header("Obstaculos (posiciones manuales)")]
    public Vector2Int[] obstaclePositions;

    [Header("Props (posiciones manuales)")]
    public Vector2Int[] propPositions;

    private bool[,] _blocked;

    void Start()
    {
        _blocked = new bool[gridWidth, gridHeight];
        BuildFloor();
        BuildObstacles();
        BuildProps();
        BuildBorders();
        PlaceGoal();
    }

    void BuildFloor()
    {
        if (floorPrefab == null) { Debug.LogWarning("LevelBuilder: floorPrefab no asignado."); return; }
        var p = new GameObject("_Floor"); p.transform.parent = transform;
        for (int x = 0; x < gridWidth; x++)
            for (int z = 0; z < gridHeight; z++)
            {
                var t = Instantiate(floorPrefab, CellToWorld(new Vector2Int(x, z)), Quaternion.identity, p.transform);
                t.transform.localScale = Vector3.one * 1.8f;
                if (floorMaterial != null)
                    foreach (var r in t.GetComponentsInChildren<Renderer>()) r.material = floorMaterial;
            }
    }

    void BuildObstacles()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;
        var p = new GameObject("_Obstacles"); p.transform.parent = transform;
        foreach (var cell in obstaclePositions)
        {
            if (!IsInBounds(cell)) continue;
            _blocked[cell.x, cell.y] = true;
            var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            if (prefab == null) continue;
            var o = Instantiate(prefab, CellToWorld(cell), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0), p.transform);
            o.transform.localScale = Vector3.one * 1.4f;
            if (obstacleMaterial != null)
                foreach (var r in o.GetComponentsInChildren<Renderer>()) r.material = obstacleMaterial;
            BoxCollider bc = o.GetComponent<BoxCollider>() ?? o.AddComponent<BoxCollider>();
            bc.size = new Vector3(cellSize * 0.85f, 2f, cellSize * 0.85f);
        }
    }

    void BuildProps()
    {
        if (propPrefabs == null || propPrefabs.Length == 0 || propPositions == null) return;
        var p = new GameObject("_Props"); p.transform.parent = transform;
        foreach (var cell in propPositions)
        {
            if (!IsInBounds(cell)) continue;
            var prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
            if (prefab == null) continue;
            var o = Instantiate(prefab, CellToWorld(cell) + Vector3.up * 0.1f, Quaternion.Euler(0, Random.Range(0, 360), 0), p.transform);
            o.transform.localScale = Vector3.one * 1.2f;
        }
    }

    void BuildBorders()
    {
        if (borderStraightPrefab == null) return;
        var p = new GameObject("_Borders"); p.transform.parent = transform;
        float h = (gridWidth - 1) * cellSize / 2f, off = cellSize * 0.6f;
        for (int i = 0; i < gridWidth; i++)
        {
            float t = i * cellSize - h;
            Sb(borderStraightPrefab, new Vector3(t, 0, h + off), 0, p);
            Sb(borderStraightPrefab, new Vector3(t, 0, -h - off), 180, p);
            Sb(borderStraightPrefab, new Vector3(h + off, 0, t), 90, p);
            Sb(borderStraightPrefab, new Vector3(-h - off, 0, t), 270, p);
        }
        if (borderCornerPrefab == null) return;
        float co = h + off;
        Sb(borderCornerPrefab, new Vector3(co, 0, co), 0, p);
        Sb(borderCornerPrefab, new Vector3(-co, 0, co), 90, p);
        Sb(borderCornerPrefab, new Vector3(-co, 0, -co), 180, p);
        Sb(borderCornerPrefab, new Vector3(co, 0, -co), 270, p);
    }

    void Sb(GameObject pf, Vector3 pos, float ry, GameObject par)
    {
        if (pf == null) return;
        var b = Instantiate(pf, pos, Quaternion.Euler(0, ry, 0), par.transform);
        b.transform.localScale = Vector3.one * 1.8f;
    }

    void PlaceGoal()
    {
        if (goalPrefab == null) return;
        Vector3 pos = CellToWorld(goalCell) + Vector3.up * 0.5f;
        Instantiate(goalPrefab, pos, Quaternion.identity, transform);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        float x = cell.x * cellSize - (gridWidth - 1) * cellSize / 2f;
        float z = cell.y * cellSize - (gridHeight - 1) * cellSize / 2f;
        return new Vector3(x, 0, z);
    }

    bool IsInBounds(Vector2Int c) => c.x >= 0 && c.x < gridWidth && c.y >= 0 && c.y < gridHeight;
    public bool IsBlocked(Vector2Int c) { if (!IsInBounds(c)) return true; return _blocked[c.x, c.y]; }
}
