using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HUD en cada nivel: muestra numero de nivel, boton de vista cenital, boton de menu.
/// </summary>
public class LevelHUD : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI levelLabel;
    public Button topDownButton;
    public Button menuButton;
    public TextMeshProUGUI topDownBtnLabel;

    [Header("Nivel actual")]
    public int levelIndex = 1;

    private CameraFollow _cam;

    void Start()
    {
        _cam = FindObjectOfType<CameraFollow>();

        if (levelLabel != null) levelLabel.text = "Nivel " + levelIndex;

        if (topDownButton != null)
            topDownButton.onClick.AddListener(ToggleTopDown);

        if (menuButton != null)
            menuButton.onClick.AddListener(() => {
                if (GameManager.Instance != null) GameManager.Instance.LoadMainMenu();
                else UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            });
    }

    void ToggleTopDown()
    {
        if (_cam == null) return;
        _cam.ToggleTopDown();
        if (topDownBtnLabel != null)
            topDownBtnLabel.text = _cam.IsTopDown ? "Vista Normal" : "Vista Cenital";
    }
}
