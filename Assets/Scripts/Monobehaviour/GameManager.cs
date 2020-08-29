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
    }

    public void Reset()
    {
        SwithUIPanels(StartGamePanel);
        Score = 0;
        LoadLevel(0);
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
     
        LoadLevel(Level + 1);
    }

    public void LoadLevel(int newLevel)
    {
        if (newLevel > 2)
        {
            Reset();
            return;
        }

        UnloadLevel();
        Level = newLevel;
        SceneManager.LoadScene("Test Level " + Level, LoadSceneMode.Additive);
        Debug.Log("Load Scene : " + Level);
    }

    public void UnloadLevel()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (Entity e in entityManager.GetAllEntities())
            entityManager.DestroyEntity(e);

        if (SceneManager.GetSceneByName("Test Level " + Level).isLoaded)
        {
            SceneManager.UnloadSceneAsync("Test Level " + Level);
            Debug.Log("Unload Scene : "+ Level);
        }
    }
}