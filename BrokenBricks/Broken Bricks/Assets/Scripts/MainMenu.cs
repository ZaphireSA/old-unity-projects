using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour {

    public GameObject stageContainer;
    public GameObject stageButtonPrefab;
    public CanvasGroup mainMenu;
    public CanvasGroup options;
    public Text txtCookies;

    private void Start()
    {
        int counter = 1;
        int prevStageStars = 0;
        foreach(Stage stage in StageManager.instance.stages)
        {
            var stageScore = StageManager.instance.GetStageScore(stage.SceneName);
            var stars = stage.CalculateStars(stageScore);
            var btn = Instantiate(stageButtonPrefab, stageContainer.transform);
            btn.transform.Find("TxtLevel").GetComponent<Text>().text = counter.ToString();
            //btn.transform.Find("TxtScore").GetComponent<Text>().text = stage.Score.ToString();

            btn.transform.Find("stars_full").Find("Star1").GetComponent<Image>().color = stars >= 1 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
            btn.transform.Find("stars_full").Find("Star2").GetComponent<Image>().color = stars >= 2 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
            btn.transform.Find("stars_full").Find("Star3").GetComponent<Image>().color = stars >= 3 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

            

            if (stage.IsDependant)
            {
                if (prevStageStars == 0)
                {
                    btn.GetComponent<Button>().interactable = false;                    
                    btn.transform.Find("lock").GetComponent<Image>().enabled = true;
                }
            }


            var sceneName = stage.SceneName;
            btn.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(sceneName); });
            counter++;
            prevStageStars = stars;
        }
        UpdateCookiesUI();
    }

    public void UpdateCookiesUI()
    {
        if (txtCookies != null)
        {
            txtCookies.text = StageManager.instance.playerData.Cookies.ToString();
        }
    }

    public void ShowAd()
    {
        AdManager.instance.ShowRewardedVideo();
    }

    public void StartGame ()
	{ 
		SceneManager.LoadScene ("Stage_1");
	}
	public void ExitGame ()
	{
		Application.Quit();
	}

	public void ChooseLevel(string name)
	{
        Debug.Log(name);
		SceneManager.LoadScene (name);

	}

    private void Update()
    {

    }

    public void MenuHide()
    {
        mainMenu.alpha = 0;      
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
        options.alpha = 1;
        options.interactable = true;
        options.blocksRaycasts = true;
    }
    public void MenuShow()
    {
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
        options.alpha = 0;
        options.interactable = false;
        options.blocksRaycasts = false;
    }
 
}

