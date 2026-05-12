using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Genera botones de seleccion de nivel dinamicamente.
/// Muestra bloqueado/desbloqueado segun PlayerPrefs.
/// </summary>
public class LevelSelectUI : MonoBehaviour
{
    [Header("Prefab del boton de nivel")]
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;
    public int totalLevels = 7;

    void OnEnable() { BuildButtons(); }

    void BuildButtons()
    {
        foreach (Transform t in buttonContainer) Destroy(t.gameObject);

        int maxUnlocked = PlayerPrefs.GetInt("MaxLevel", 1);

        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject btn = Instantiate(levelButtonPrefab, buttonContainer);
            btn.name = "LevelBtn_" + i;

            TextMeshProUGUI label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label == null) { var t = btn.GetComponentInChildren<Text>(); if (t != null) t.text = "Nivel " + i; }
            else label.text = "Nivel " + i;

            bool unlocked = i <= maxUnlocked;
            Button button = btn.GetComponent<Button>();

            if (button != null)
            {
                button.interactable = unlocked;
                int levelNum = i;
                button.onClick.AddListener(() => LoadLevel(levelNum));
            }

            // Icono de candado
            Transform lockIcon = btn.transform.Find("LockIcon");
            if (lockIcon != null) lockIcon.gameObject.SetActive(!unlocked);
        }
    }

    void LoadLevel(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + index);
    }
}
