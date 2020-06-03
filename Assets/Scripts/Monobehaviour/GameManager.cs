using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public int Score, Level, CurrentLives;
    public TextMeshProUGUI PickupText, ScoreText;
    public GameObject StartGamePanel, GamePanel, WinPanel, LosePanel;

    public void Awake()
    {
        Instance = this;
        Reset();
        NextLevel();
    }

    public void Reset()
    {
        SwithUIPanels(StartGamePanel);
        Score = 0;
    }

    public void InGame()
    {
        SwithUIPanels(GamePanel);
    }

    public void Win()
    {
        SwithUIPanels(WinPanel);
    }

    public void GameOver()
    {
        SwithUIPanels(LosePanel);
    }

    public void SubtractLife()
    {

        CurrentLives--;
        if (CurrentLives < 0)
        {
            GameOver();
        }
    }

    public void SwithUIPanels(GameObject panel)
    {
        StartGamePanel.SetActive(false);
        GamePanel.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);

        panel.SetActive(true);
    }

    public void AddPoints(int points)
    {
        Score += points;
        ScoreText.text = "Score : " + Score;
    }

    public void SetDotsCount(int dotsCount)
    {
        if (!GamePanel.activeInHierarchy)
            return;

        PickupText.text = "DOTS : " + dotsCount;

        if (dotsCount == 0)
            Win();
    }

    public void NextLevel()
    {
        InGame();
    }


}