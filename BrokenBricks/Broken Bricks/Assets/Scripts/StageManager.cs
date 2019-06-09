using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class StageManager : MonoBehaviour
{

    public static StageManager instance;

    public List<Stage> stages = new List<Stage>();    
    //public List<Spell> spells = new List<Spell>();
    public PlayerData playerData = new PlayerData();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadPlayerData();
            QualitySettings.SetQualityLevel(playerData.QualityLevel);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        Input.simulateMouseWithTouches = true;
#endif
    }

    public void LoadPlayerData()
    {        
        //File.Delete(Application.persistentDataPath + "/playerdata.save");
        if (File.Exists(Application.persistentDataPath + "/playerdata.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerdata.save", FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }

    public void AddCookies(int amount)
    {
        playerData.Cookies += amount;
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/playerdata.save"); //you can call it anything you want
        bf.Serialize(file, playerData);
        file.Close();
    }

    //public void LoadData()
    //{        
    //    if (File.Exists(Application.persistentDataPath + "/stagescores.save"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "/stagescores.save", FileMode.Open);
    //        stagesScore = (List<StageScore>)bf.Deserialize(file);
    //        file.Close();
    //    }
    //}


    //public void SaveData ()
    //{
    //    BinaryFormatter bf = new BinaryFormatter();
    //    //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
    //    FileStream file = File.Create(Application.persistentDataPath + "/stagescores.save"); //you can call it anything you want
    //    bf.Serialize(file, stagesScore);
    //    file.Close();
    //}

    public void UpdateScore(int score)
    {
        var stage = GetCurrentStage();
        var stageScore = playerData.stagesScore.Where(x => x.SceneName == stage.SceneName).FirstOrDefault();
        if (stageScore == null)
        {
            stageScore = new StageScore
            {
                SceneName = stage.SceneName,
                Score = 0
            };
            playerData.stagesScore.Add(stageScore);
        }
        if (score > stageScore.Score)
        stageScore.Score = score;
        SavePlayerData();
    }

    public void SetBackgroundMusic(bool value)
    {
        playerData.BackgroundMusic = value;
        SavePlayerData();
        AudioManager.instance.ToggleBackgroundVolume(value);
    }

    public void SetQualityLevel(int level)
    {
        playerData.QualityLevel = level;
        SavePlayerData();
    }

    public Stage GetCurrentStage()
    {
        return stages.FirstOrDefault(x => x.SceneName == SceneManager.GetActiveScene().name);
    }


    public Stage GetNextStage()
    {
        Stage currentStage = GetCurrentStage();
        var index = stages.IndexOf(currentStage) + 1; 
        if (stages.Count > index)
        {
            return stages[index];
        } else
        {
            return null;
        }        
    }

    //public void BuySpell(Spell spell)
    //{
    //    var unlockedSpell = playerData.Spells.Where(x => x.SpellName == spell.Name).FirstOrDefault();
    //    AddCookies(-spell.Cost);
    //    if (unlockedSpell != null)
    //    {
    //        unlockedSpell.Charges++;
    //    } else
    //    {
    //        unlockedSpell = new UnlockedSpell { SpellName = spell.Name, Charges = 1 };
    //        playerData.Spells.Add(unlockedSpell);
    //    }
    //    SavePlayerData();
    //}

    //public bool IsSpellUnlocked(Spell spell)
    //{
    //    var unlockedSpell = playerData.Spells.Where(x => x.SpellName == spell.Name).FirstOrDefault();
    //    return unlockedSpell != null;
    //}

    //public int GetSpellCharges(Spell spell)
    //{
    //    var unlockedSpell = playerData.Spells.Where(x => x.SpellName == spell.Name).FirstOrDefault();
    //    return unlockedSpell == null ? 0 : unlockedSpell.Charges;
    //}

    //public void UseSpell(string spellName)
    //{
    //    var unlockedSpell = playerData.Spells.Where(x => x.SpellName == spellName).FirstOrDefault();
    //    unlockedSpell.Charges--;
    //    SavePlayerData();
    //}

    public int GetStageScore(string sceneName)
    {
        var stageScore = playerData.stagesScore.Where(x => x.SceneName == sceneName).FirstOrDefault();
        return stageScore == null ? -1 : stageScore.Score;        
    }
}

[System.Serializable]
public class PlayerData
{
    public int Cookies = 0;
    //public List<UnlockedSpell> Spells = new List<UnlockedSpell>();
    public List<StageScore> stagesScore = new List<StageScore>();
    public bool BackgroundMusic = true;
    public int QualityLevel = 3;
}

[System.Serializable]
public class Stage
{
    public string SceneName;
    public int MaxScore = 0;
    //public int Star1Score => 
    //public int Star2Score = 0;
    //public int Star3Score = 0;
    public bool IsDependant = true;


    public int Star1Score
    {
        get { return Mathf.RoundToInt(0);}     
    }

    public int Star2Score
    {
        get { return Mathf.RoundToInt(MaxScore * 0.6f); }
    }

    public int Star3Score
    {
        get { return Mathf.RoundToInt(MaxScore * 0.85f); }
    }

    public int CalculateStars(int score)
    {
        if (score >= Star3Score)
        {
            return 3;
        }
        else if (score >= Star2Score)
        {
            return 2;
        }
        else if (score >= Star1Score)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
}

[System.Serializable]
public class StageScore
{
    public string SceneName;
    public int Score = 0;
}


//[System.Serializable]
//public class Spell
//{
//    public string Name;
//    public string Description;
//    public Sprite Icon;
//    public int Cost;
//    public int MaxCharges;
//    public int RequiredStage = 0;
//}

//[System.Serializable]
//public class UnlockedSpell
//{
//    public string SpellName;
//    public int Charges;
//}