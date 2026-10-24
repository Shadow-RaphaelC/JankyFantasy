using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Ecran de fin")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [Header("Boss")]
    [SerializeField] private Boss boss;

    private List<Character> players = new List<Character>();

    private bool gameOver = false;

    private void Start()
    {
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        if (boss == null) return;
    }

    private void Update()
    {
        if (gameOver) return;

        if (boss != null && boss.CurrentHP <= 0)
        {
            Win();
            return;
        }
        
        if (AllPlayerDowned())
        {
            Lose();
        }
    }

    public void RegisterPlayer(Character player)
    {
        players.Add(player);
    }

    private bool AllPlayerDowned()
    {
        if (players.Count == 0) return false;
        foreach (Character p in players)
        {
            if (!p.IsDowned) return false;
        }
        return true;
    }

    private void Win()
    {
        gameOver = true;
        if (winScreen != null) winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Lose()
    {
        gameOver = true;
        if (loseScreen != null) loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterSelectScene");
    }
}
