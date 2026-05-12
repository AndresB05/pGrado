using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el menu principal: jugar, seleccion de nivel, resetear progreso.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        int max = PlayerPrefs.GetInt("MaxLevel", 1);
        SceneManager.LoadScene("Level" + max);
    }

    public void GoToLevelSelect()
    {
        // La UI de seleccion de nivel esta en la misma escena, activar panel
        var panel = GameObject.Find("LevelSelectPanel");
        if (panel != null) panel.SetActive(true);
        var main = GameObject.Find("MainPanel");
        if (main != null) main.SetActive(false);
    }

    public void BackToMain()
    {
        var panel = GameObject.Find("LevelSelectPanel");
        if (panel != null) panel.SetActive(false);
        var main = GameObject.Find("MainPanel");
        if (main != null) main.SetActive(true);
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("MaxLevel", 1);
        PlayerPrefs.Save();
        Debug.Log("Progreso reseteado.");
    }

    public void QuitGame() { Application.Quit(); }
}
