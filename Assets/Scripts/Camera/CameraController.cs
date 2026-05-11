using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Cámara orbital que orbita alrededor del centro del tablero.
/// Click derecho + arrastrar para rotar. Scroll para zoom.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;              // Centro del tablero

    [Header("Órbita")]
    public float distance = 12f;
    public float minDistance = 5f;
    public float maxDistance = 25f;
    public float orbitSpeed = 120f;
    public float zoomSpeed = 4f;

    [Header("Ángulo vertical")]
    public float verticalAngle = 45f;
    public float minVerticalAngle = 10f;
    public float maxVerticalAngle = 80f;

    private float _horizontalAngle = 45f;
    private Vector2 _lastMousePos;
    private bool _isDragging = false;

    void LateUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Click derecho para orbitar
        if (mouse.rightButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _lastMousePos = mouse.position.ReadValue();
        }

        if (mouse.rightButton.wasReleasedThisFrame)
            _isDragging = false;

        if (_isDragging)
        {
            Vector2 delta = mouse.position.ReadValue() - _lastMousePos;
            _lastMousePos = mouse.position.ReadValue();

            _horizontalAngle += delta.x * orbitSpeed * Time.deltaTime;
            verticalAngle -= delta.y * orbitSpeed * Time.deltaTime;
            verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        // Scroll para zoom
        float scroll = mouse.scroll.ReadValue().y;
        distance -= scroll * zoomSpeed * Time.deltaTime * 10f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calcular posición orbital
        Quaternion rotation = Quaternion.Euler(verticalAngle, _horizontalAngle, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 targetPos = target != null ? target.position : Vector3.zero;

        transform.position = targetPos + offset;
        transform.LookAt(targetPos);
    }
}
