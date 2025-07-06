using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    
    [Header("Panels")] [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gamePanel;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeText;
    
    
    private void Start()
    {
        InitializePanels();
    }

    private void InitializePanels()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameManager.InitializeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void MainMenuButton()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void OnVolumeChanged()
    {
        volumeText.text = $"{volumeSlider.value * 100}%";
    }
}
