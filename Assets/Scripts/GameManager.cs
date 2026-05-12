using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton que persiste entre escenas. Gestiona progresion y transiciones.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuracion")]
    public int totalLevels = 7;

    public int MaxLevelUnlocked
    {
        get { return PlayerPrefs.GetInt("MaxLevel", 1); }
        private set { PlayerPrefs.SetInt("MaxLevel", value); PlayerPrefs.Save(); }
    }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= MaxLevelUnlocked)
            MaxLevelUnlocked = levelIndex + 1;

        string next = levelIndex >= totalLevels ? "WinScreen" : "Level" + (levelIndex + 1);
        SceneManager.LoadScene(next);
    }

    public void LoadLevel(int index) { SceneManager.LoadScene("Level" + index); }
    public void LoadMainMenu() { SceneManager.LoadScene("MainMenu"); }
    public void LoadWinScreen() { SceneManager.LoadScene("WinScreen"); }
    public bool IsLevelUnlocked(int index) { return index <= MaxLevelUnlocked; }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("MaxLevel", 1);
        PlayerPrefs.Save();
    }
}
