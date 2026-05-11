using UnityEngine;
using UnityEngine.InputSystem;

public class CuboMovimiento : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float x = 0f;
        float y = 0f;
        float z = 0f;

        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.eKey.isPressed) y += 1f;
        if (keyboard.qKey.isPressed) y -= 1f;

        Vector3 movimiento = new Vector3(x, y, z).normalized * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }
}
