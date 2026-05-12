using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelHUDRuntime : MonoBehaviour
{
    public int levelIndex = 1;
    private CameraFollow _cam;

    void Start()
    {
        _cam = FindObjectOfType<CameraFollow>();

        var btnCenital = transform.Find("TopPanel/BtnCenital")?.GetComponent<Button>();
        if (btnCenital != null) btnCenital.onClick.AddListener(ToggleTopDown);

        var btnMenu = transform.Find("TopPanel/BtnMenu")?.GetComponent<Button>();
        if (btnMenu != null) btnMenu.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleTopDown();
    }

    void ToggleTopDown()
    {
        if (_cam == null) _cam = FindObjectOfType<CameraFollow>();
        if (_cam != null) _cam.ToggleTopDown();

        var btn = transform.Find("TopPanel/BtnCenital")?.GetComponentInChildren<Text>();
        if (btn != null) btn.text = _cam != null && _cam.IsTopDown ? "Vista Normal" : "Vista Cenital";
    }
}