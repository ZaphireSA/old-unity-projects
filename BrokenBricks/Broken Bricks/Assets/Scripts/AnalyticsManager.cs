using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var currentStage = StageManager.instance.GetCurrentStage();
        if (currentStage != null)
        {
            LevelStart(currentStage);
        }
    }


    public void LevelStart(Stage stage)
    {
        var indexName = stage.SceneName.Replace("Stage_", "");
        AnalyticsEvent.Custom("level_start", new Dictionary<string, object>
                {
                    { "level_index", indexName }
                });
    }

    public void LevelComplete(Stage stage)
    {

        var indexName = stage.SceneName.Replace("Stage_", "");


        AnalyticsEvent.Custom("level_complete", new Dictionary<string, object>
                {
                    { "level_index", indexName }
                });
    }
}
