using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameData : MonoBehaviour {

    public static GameData instance;
    public PlayerData playerData = new PlayerData();

    public event System.Action<int> CoinsChange;
    public event System.Action<int> ScrapChange;

    void Awake()
    {
        if (instance != null)
        {
            instance.CoinsChange = null;
            instance.ScrapChange = null;
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LoadAllData();
    }

    void LoadAllData()
    {
        Load();
        
    }

    public void Save()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        formatter.Serialize(saveFile, playerData);

        saveFile.Close();
    }

    public void Load()
    {
        if(File.Exists("Saves/save.binary"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
            playerData = (PlayerData)formatter.Deserialize(saveFile);
            saveFile.Close();
        }
    }    

    public void AddCoins(int amount)
    {
        playerData.totalCoins += amount;
        Save();
        if (CoinsChange != null)
            CoinsChange(playerData.totalCoins);
    }

    public bool UseCoins(int amount)
    {
        if ((playerData.totalCoins - amount) >= 0)
        {
            playerData.totalCoins -= amount;
            Save();
            if (CoinsChange != null)
                CoinsChange(playerData.totalCoins);
            return true;
        }
        return false;
    }

    public void AddScrap(int amount)
    {
        playerData.totalScrap += amount;
        Save();
        if (ScrapChange != null)
            ScrapChange(playerData.totalScrap);
    }

    public bool UseScrap(int amount)
    {
        if ((playerData.totalScrap - amount) >= 0)
        {
            playerData.totalScrap -= amount;
            Save();
            if (ScrapChange != null)
                ScrapChange(playerData.totalScrap);
            return true;
        }
        return false;
    }

    public int CoinAmount()
    {
        return playerData.totalCoins;
    }

    public int ScrapAmount()
    {
        return playerData.totalScrap;
    }

    [System.Serializable]
    public class PlayerData
    {
        public int totalCoins = 0;
        public int totalScrap = 0;

        public int weapon1Id;
        public int weapon2Id;

        public PlayerData()
        {
            totalCoins = 0;
            totalScrap = 0;
            weapon1Id = -1;
            weapon2Id = -1;
        }
    }
    
}
