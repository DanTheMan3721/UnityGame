using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup UIGroup;
    private bool fade;

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Setup()
    {
        fade = true;
    }

    private void Update()
    {
        if (UIGroup.alpha < 1 && fade)
        {
            UIGroup.alpha += Time.deltaTime;
        }
    }
}
