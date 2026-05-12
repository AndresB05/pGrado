using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float cellSize = 2f;
    public int gridWidth = 4;
    public int gridHeight = 4;
    public Vector2Int startCell = new Vector2Int(0, 0);

    private Vector2Int _currentCell;
    private bool _isMoving = false;

    void Start()
    {
        _currentCell = startCell;
        transform.position = CellToWorld(_currentCell) + Vector3.up * 0.5f;
    }

    void Update()
    {
        if (_isMoving) return;
        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))         dir = new Vector2Int(0, 1);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))  dir = new Vector2Int(0, -1);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))  dir = new Vector2Int(-1, 0);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) dir = new Vector2Int(1, 0);
        if (dir == Vector2Int.zero) return;
        Vector2Int target = _currentCell + dir;
        if (IsInBounds(target)) StartCoroutine(MoveToCell(target));
    }

    IEnumerator MoveToCell(Vector2Int target)
    {
        _isMoving = true;
        Vector3 start = transform.position;
        Vector3 end = CellToWorld(target) + Vector3.up * 0.5f;
        float elapsed = 0f, duration = 1f / moveSpeed;
        Vector3 d = new Vector3(end.x - start.x, 0, end.z - start.z).normalized;
        if (d != Vector3.zero) transform.rotation = Quaternion.LookRotation(d);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, elapsed / duration));
            yield return null;
        }
        transform.position = end;
        _currentCell = target;
        _isMoving = false;
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        float x = cell.x * cellSize - (gridWidth - 1) * cellSize / 2f;
        float z = cell.y * cellSize - (gridHeight - 1) * cellSize / 2f;
        return new Vector3(x, 0, z);
    }

    bool IsInBounds(Vector2Int c) => c.x >= 0 && c.x < gridWidth && c.y >= 0 && c.y < gridHeight;
    public Vector2Int CurrentCell => _currentCell;
}
