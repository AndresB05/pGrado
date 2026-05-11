using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;

    [Header("Posicion inicial")]
    public Vector2Int startCell = new Vector2Int(0, 0);

    [Header("Modelo visual (arrastra character-soldier FBX aqui)")]
    public GameObject characterModelPrefab;

    private Vector2Int _currentCell;
    private bool _isMoving = false;

    void Start()
    {
        _currentCell = startCell;
        transform.position = GridManager.Instance.GridToWorld(_currentCell) + Vector3.up * 0.05f;

        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();
        var col = GetComponent<CapsuleCollider>();
        if(mf != null) Destroy(mf);
        if(mr != null) Destroy(mr);
        if(col != null) Destroy(col);

        GameObject modelPrefab = characterModelPrefab;
        if(modelPrefab == null)
            modelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Kenney/miniArena/FBX format/character-soldier.fbx");

        if(modelPrefab != null)
        {
            GameObject model = Instantiate(modelPrefab, transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one * 3.5f;
        }
        else
        {
            Debug.LogWarning("No se encontro character-soldier.fbx");
        }

        CapsuleCollider newCol = gameObject.AddComponent<CapsuleCollider>();
        newCol.height = 2f;
        newCol.radius = 0.4f;
        newCol.center = new Vector3(0, 1f, 0);
    }

    void Update()
    {
        if(_isMoving) return;
        var keyboard = Keyboard.current;
        if(keyboard == null) return;

        Vector2Int direction = Vector2Int.zero;
        if(keyboard.wKey.wasPressedThisFrame)      direction = new Vector2Int(0, 1);
        else if(keyboard.sKey.wasPressedThisFrame) direction = new Vector2Int(0, -1);
        else if(keyboard.aKey.wasPressedThisFrame) direction = new Vector2Int(-1, 0);
        else if(keyboard.dKey.wasPressedThisFrame) direction = new Vector2Int(1, 0);

        if(direction == Vector2Int.zero) return;

        Vector2Int targetCell = _currentCell + direction;
        if(!GridManager.Instance.IsBlocked(targetCell))
            StartCoroutine(MoveToCell(targetCell));
    }

    IEnumerator MoveToCell(Vector2Int targetCell)
    {
        _isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = GridManager.Instance.GridToWorld(targetCell) + Vector3.up * 0.05f;
        float elapsed = 0f;
        float duration = 1f / moveSpeed;

        Vector3 dir = new Vector3(endPos.x - startPos.x, 0, endPos.z - startPos.z).normalized;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        _currentCell = targetCell;
        _isMoving = false;
    }

    public Vector2Int CurrentCell => _currentCell;
}
