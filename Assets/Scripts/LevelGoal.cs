using System.Collections;
using UnityEngine;

/// <summary>
/// Zona de meta. Al entrar el jugador, completa el nivel.
/// </summary>
public class LevelGoal : MonoBehaviour
{
    [Header("Nivel que completa (1-7)")]
    public int levelIndex = 1;

    [Header("Efectos")]
    public float completionDelay = 1.5f;
    public GameObject goalVFX;
    public AudioClip completionSound;

    private bool _triggered = false;
    private AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (goalVFX != null) goalVFX.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;
        _triggered = true;
        StartCoroutine(CompleteRoutine());
    }

    IEnumerator CompleteRoutine()
    {
        if (_audio != null && completionSound != null)
            _audio.PlayOneShot(completionSound);

        yield return new WaitForSeconds(completionDelay);

        if (GameManager.Instance != null)
            GameManager.Instance.CompleteLevel(levelIndex);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
