using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Pantalla de victoria final.
/// </summary>
public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    void Start()
    {
        if (titleText != null) titleText.text = "¡Felicidades!";
        if (subtitleText != null) subtitleText.text = "Completaste todos los niveles";
    }

    public void BackToMenu() { SceneManager.LoadScene("MainMenu"); }
    public void PlayAgain()
    {
        PlayerPrefs.SetInt("MaxLevel", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }
}
