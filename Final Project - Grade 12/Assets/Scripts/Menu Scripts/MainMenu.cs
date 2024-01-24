using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Forest");
    }
    public void levelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void options()
    {
        SceneManager.LoadScene("Options");
    }
        public void quitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
