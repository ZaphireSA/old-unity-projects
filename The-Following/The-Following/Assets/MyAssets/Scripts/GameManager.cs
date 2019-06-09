using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int SoulsCollected = 0;
    public float TimeLeft = 60f;
    [SerializeField] Text txtLife;
    [SerializeField] Text txtSouls;
    [SerializeField] Text txtSouls2;
    [SerializeField] CanvasGroup gameOverPanel;

    bool IsGameOver = false;

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
        txtLife.text = TimeLeft.ToString("0");
        if (!IsGameOver && TimeLeft <= 0)
        {
            IsGameOver = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        txtSouls2.text = SoulsCollected.ToString();
        gameOverPanel.alpha = 1;
        gameOverPanel.blocksRaycasts = true;
        gameOverPanel.interactable = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void AddSoul()
    {
        SoulsCollected++;
        txtSouls.text = SoulsCollected.ToString();
        TimeLeft += 5;
    }
}
