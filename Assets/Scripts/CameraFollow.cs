using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 8, -10);
    public float smoothSpeed = 6f;
    public float rotationSpeed = 4f;
    public Vector3 topDownOffset = new Vector3(0, 18, 0);
    public KeyCode topDownKey = KeyCode.Tab;

    private bool _isTopDown = false;
    private Vector3 _currentOffset;

    void Start()
    {
        _currentOffset = offset;
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (Input.GetKeyDown(topDownKey)) _isTopDown = !_isTopDown;

        Vector3 desiredOffset = _isTopDown ? topDownOffset : offset;
        _currentOffset = Vector3.Lerp(_currentOffset, desiredOffset, Time.deltaTime * smoothSpeed);

        Vector3 desiredPos = target.position + _currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);

        if (_isTopDown)
        {
            // Vista cenital FIJA - no rota con el personaje
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * rotationSpeed);
        }
        else
        {
            Vector3 lookTarget = target.position + Vector3.up * 1f;
            Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationSpeed);
        }
    }

    public void ToggleTopDown() { _isTopDown = !_isTopDown; }
    public bool IsTopDown => _isTopDown;
}
