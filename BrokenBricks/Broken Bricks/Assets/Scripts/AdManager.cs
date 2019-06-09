using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

    public static AdManager instance;
    private string gameId = "1733860";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Advertisement.Initialize(gameId, false);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    public void ShowRewardedVideo()
    {
        Debug.Log("Starting Ad");
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                StageManager.instance.AddCookies(20);
                if (GameManager.instance) GameManager.instance.RewardCollected();
                Debug.Log("Rewarded 20 cookies");
                break;
            case ShowResult.Failed:
                Debug.Log("Failed to show video");
                break;
            case ShowResult.Skipped:
                Debug.Log("Skipped the video");
                break;
        }
    }    

    public void ContinueGameOverVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = ContinueGameOverResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void ContinueGameOverResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //StageManager.instance.AddCookies(20);
                GameManager.instance.ContinueFromGameover();
                break;
            case ShowResult.Failed:
                Debug.Log("Failed to show video");
                break;
            case ShowResult.Skipped:
                Debug.Log("Skipped the video");
                break;
        }
    }
}
