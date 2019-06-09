using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHandler : MonoBehaviour {

    public GameObject playerUI, menuUI;
    public TextMeshProUGUI topScore, prevScore, lblPrevScore, totalDistanceTravelled;


    public void Start()
    {
        topScore.text = GooglePlayManager.instance.playerData.MaxDistance.ToString();
        totalDistanceTravelled.text = GooglePlayManager.instance.playerData.TotalDistanceTravelled.ToString();
        if (GooglePlayManager.instance.lastScore <= 0)
        {
            prevScore.gameObject.SetActive(false);
            lblPrevScore.gameObject.SetActive(false);
        } else
        {
            prevScore.text = GooglePlayManager.instance.lastScore.ToString();
        }
    }

    public void ShowLeaderboard()
    {
        GooglePlayManager.ShowLeaderboardsUI();
    }

    public void ShowAchievements()
    {
        GooglePlayManager.ShowAchievementsUI();
    }

    public void StartGame()
    {
        //Time.timeScale = 1;
        FindObjectOfType<Cube>().Activate();
        menuUI.SetActive(false);
        playerUI.SetActive(true);
        //AdManager.instance.ShowBannerAd();
    }
}
