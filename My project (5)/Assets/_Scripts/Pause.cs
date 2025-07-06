using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gamePanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OptionsButton()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void MainMenu()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}