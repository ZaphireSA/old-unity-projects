using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour {

    public static GooglePlayManager instance;
    public PlayerData playerData = new PlayerData();
    public long lastScore = 0;

    public int attempts = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
            LoadPlayerData();

        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignIn();
    }

    public static void SignIn()
    {
        //PlayGamesPlatform.Instance.localUser.Authenticate(SignInSuccessfulCallBack);
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
        });
    }

    private static void SignInSuccessfulCallBack(bool obj)
    {
        Debug.Log("Login: " + obj);
        //throw new NotImplementedException();
    }

    #region Achievements
    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, AchievementUnlockedCallBack);
    }

    private static void AchievementUnlockedCallBack(bool obj)
    {
      
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }

    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }
    #endregion

    #region Leaderboards

    public static void AddScoreToLeaderboard(string boardId, long score)
    {
        //Distance Travelled
        //PlayGamesPlatform.Instance.ReportScore(score, boardId, success => { });
        Social.ReportScore(score, boardId, (bool success) => {
            // handle success or failure
        });

        
    }

    public static void ShowLeaderboardsUI()
    {        
        Social.ShowLeaderboardUI();
        //PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }

    #endregion


    public void LoadPlayerData()
    {
        //File.Delete(Application.persistentDataPath + "/playerdata.save");
        if (File.Exists(Application.persistentDataPath + "/playerdata.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerdata.save", FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
            if (playerData.TotalDistanceTravelled < playerData.MaxDistance) playerData.TotalDistanceTravelled = playerData.MaxDistance;
            if (playerData.TotalDistanceTravelled >= 2147483647) playerData.TotalDistanceTravelled = playerData.MaxDistance;
        }
    }

    public void SavePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/playerdata.save"); //you can call it anything you want
        bf.Serialize(file, playerData);
        file.Close();
    }

    public void UpdateStats(long distance, int healthPickups)
    {
        attempts++;
        lastScore = distance;
        playerData.TotalDistanceTravelled += distance;
        if (distance > playerData.MaxDistance) playerData.MaxDistance = distance;                    
        SavePlayerData();
        UpdateDistanceAchievements(distance);
        UpdateHealthAchievements(healthPickups);
        AddScoreToLeaderboard(GPGSIds.leaderboard_max_distance, Mathf.RoundToInt(distance));
        AddScoreToLeaderboard(GPGSIds.leaderboard_total_distance_travelled, Mathf.RoundToInt(playerData.TotalDistanceTravelled));
        if (attempts >= 3)
        {
            attempts = 0;
            AdManager.instance.ShowBannerAd();
        }
    }


    static void UpdateHealthAchievements(int amount)
    {
        if (amount >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_5_health_blocks, 100, AchievementUnlockedCallBack);
        }
        if (amount >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_10_health_blocks, 100, AchievementUnlockedCallBack);
        }
        if (amount >= 20)
        {
            Social.ReportProgress(GPGSIds.achievement_20_health_blocks, 100, AchievementUnlockedCallBack);
        }
        if (amount >= 40)
        {
            Social.ReportProgress(GPGSIds.achievement_40_health_blocks, 100, AchievementUnlockedCallBack);
        }


    }

    static void UpdateDistanceAchievements(long distance)
    {
        if (distance >= 500)
        {
            Social.ReportProgress(GPGSIds.achievement_baby_steps__reach_500_meters, 100, AchievementUnlockedCallBack);
        }
        
        if (distance >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievement_1000_meters, 100, AchievementUnlockedCallBack);
        }

        if (distance >= 2000)
        {
            Social.ReportProgress(GPGSIds.achievement_2000_meters, 100, AchievementUnlockedCallBack);
        }

        if (distance >= 3000)
        {
            Social.ReportProgress(GPGSIds.achievement_3000_meters, 100, AchievementUnlockedCallBack);
        }

        if (distance >= 4000)
        {
            Social.ReportProgress(GPGSIds.achievement_4000_meters, 100, AchievementUnlockedCallBack);
        }

        if (distance >= 5000)
        {
            Social.ReportProgress(GPGSIds.achievement_5000_meters, 100, AchievementUnlockedCallBack);
        }

        var distToAMil = instance.playerData.TotalDistanceTravelled / 1000000;

        Social.ReportProgress(GPGSIds.achievement_meeting_the_millions, distToAMil, AchievementUnlockedCallBack);        
    }
}


[System.Serializable]
public class PlayerData
{
    public long MaxDistance = 0;
    public long TotalDistanceTravelled = 0;
}