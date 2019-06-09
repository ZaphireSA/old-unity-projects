using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public static GameManager instance = null;
    public Text txtScore;
    public Text txtCookies;
    public int ballsLeft = 3;
    public List<GameObject> aliveBalls = new List<GameObject>();
    public GameObject ballPrefab;
    public GameObject collectEffect;
    public Text txtBalls;
    public CanvasGroup rewardUI, winUI, loseUI, pauseMenu, menu_Return, options_Menu, pause_Btn, shopUI, playerUI;
    
    public Button btnRewardVideo;
    public GameObject ballUiPrefab;
    public Transform ballUiHolder;
    public Sprite pauseSprite;
    public Sprite resumeSprite;
    public Sprite lockSprite;
    public Image pauseBtnImage;
    public Transform star1;
    public Transform star2;
    public Transform star3;
    public Transform next_btn;
    public Transform SpellPanel;
    public Transform SpellUIPrefab;
    public List<PowerupInfo> availablePowerups = new List<PowerupInfo>();
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Transform movementTracker;
    public Dropdown qualityDropDown;
    public Button btnShoot;

    public int bricksLeft = 0;
    public bool isPaused = false;

    private Breaker breaker;
    //private string selectedSpell = "";

    //public GameObject sloMoPrefab;
    //public Vector3 sloMoSpawn;

    //GameObject sloMo = null;

    void Start()
    {
        resolutions = Screen.resolutions;
        breaker = FindObjectOfType<Breaker>();
        SpawnBall();
        UpdateScoreUI();
        UpdateCookiesUI();
        qualityDropDown.value = QualitySettings.GetQualityLevel();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        //sloMo = GameObject.FindGameObjectWithTag("SloMo");
        //if (sloMo) sloMo.SetActive(false);
        //GenerateSpells();
    }

    public void UnstuckStart()
    {
        FindObjectOfType<Ball>().UnStuck();
        //btnShoot.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        var pos = movementTracker.transform.position;
        var targetPos = breaker.transform.position;
        targetPos.x = breaker.targetX;
        pos.x = Camera.main.WorldToScreenPoint(targetPos).x;
        if (breaker.state == Breaker.BreakerState.Aim)
        {
            movementTracker.transform.localPosition = Vector3.zero;
        }
        else
        {
            movementTracker.transform.position = pos;
        }
    }



    public PowerupInfo GetRandomPowerUp()
    {
        if (availablePowerups.Count <= 0) return null;

        int range = 0;

        foreach (PowerupInfo powerup in availablePowerups)
        {
            range += powerup.rarityWeight;
        }

        int randomNumber = Random.Range(0, range);

        int top = 0;

        foreach (PowerupInfo powerup in availablePowerups)
        {
            top += powerup.rarityWeight;
            if (randomNumber < top)
                return powerup;
        }
        return null;
    }

    public void BrickAdded()
    {
        bricksLeft++;
    }

    public void BrickDestroyed()
    {
        bricksLeft--;
        if (bricksLeft <= 0)
        {
            StartCoroutine(Win());
        }
    }

    public void UpdateHighscore()
    {
        StageManager.instance.UpdateScore(score);
    }

    public IEnumerator Win()
    {
        var analytics = FindObjectOfType<AnalyticsManager>();
        if (analytics != null)
        {
            var currentStage = StageManager.instance.GetCurrentStage();
            if (currentStage != null)
            {
                analytics.LevelComplete(currentStage);
            }            
        }

        var balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
        {
            Destroy(ball.gameObject);
        }

        var powerups = FindObjectsOfType<Powerup>();
        foreach(var powerup in powerups)
        {
            if (powerup is SweetCollection || powerup is Powerup_Cookie) continue;
            Destroy(powerup.gameObject);
        }


        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            var collections = FindObjectsOfType<SweetCollection>();
            if (collections.Length == 0) break;
            yield return new WaitForSeconds(0.1f);
        }


        yield return new WaitForSeconds(0.3f);
        pause_Btn.alpha = 0;
        pause_Btn.interactable = false;
        pause_Btn.blocksRaycasts = false;
        winUI.alpha = 1;
        winUI.interactable = true;
        winUI.blocksRaycasts = true;
        playerUI.alpha = 0;
        playerUI.interactable = false;
        playerUI.blocksRaycasts = false;

        var stars = StageManager.instance.GetCurrentStage().CalculateStars(score);
        var stage = StageManager.instance.GetNextStage();
        next_btn.gameObject.SetActive(stage != null);

        StartCoroutine(StarsAnimation(stars));



        Time.timeScale = 0f;

        UpdateHighscore();

    }

    IEnumerator StarsAnimation(int stars)
    {
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        AudioManager.instance.Play("win");

        if (stars >= 1)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            star1.gameObject.SetActive(true);
            AudioManager.instance.Play("star", 1f);
        }

        if (stars >= 2)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            star2.gameObject.SetActive(true);
            AudioManager.instance.Play("star", 1.1f);
        }

        if (stars >= 3)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            star3.gameObject.SetActive(true);
            AudioManager.instance.Play("star", 1.2f);
        }

    }

    public void NextStage()
    {
        var stage = StageManager.instance.GetNextStage();
        if (stage == null) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(stage.SceneName);
    }


    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        var stage = StageManager.instance.GetCurrentStage();
        SceneManager.LoadScene(stage.SceneName);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        pause_Btn.alpha = 0;
        pause_Btn.interactable = false;
        pause_Btn.blocksRaycasts = false;
        loseUI.alpha = 1;
        loseUI.interactable = true;
        loseUI.blocksRaycasts = true;
        playerUI.alpha = 0;
        playerUI.interactable = false;
        playerUI.blocksRaycasts = false;

        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);

        Time.timeScale = 0f;
        AudioManager.instance.Play("GameOver");
        //UpdateHighscore();
        //txtScoreGameOver.text = "Total Score: " + score.ToString ();
        //gameOverUI.alpha = 1; 
        //gameOverUI.interactable = true;
        //gameOverUI.blocksRaycasts = true;
        //Time.timeScale = 0f;
        //      UpdateHighscore();

    }

    public void ShowGameoverContinueAd()
    {
        AdManager.instance.ContinueGameOverVideo();
    }

    public void ContinueFromGameover()
    {
        pause_Btn.alpha = 1;
        pause_Btn.interactable = true;
        pause_Btn.blocksRaycasts = true;
        loseUI.alpha = 0;
        loseUI.interactable = false;
        loseUI.blocksRaycasts = false;
        playerUI.alpha = 1;
        playerUI.interactable = true;
        playerUI.blocksRaycasts = true;
        ballsLeft++;
        UpdateBallsUI();

        Time.timeScale = 1;
        ResetPlayer();
    }

    public void WatchVideoForReward()
    {
        AdManager.instance.ShowRewardedVideo();
    }

    public void RewardCollected()
    {
        rewardUI.alpha = 1;
        rewardUI.interactable = true;
        rewardUI.blocksRaycasts = true;
        btnRewardVideo.interactable = false;
        AudioManager.instance.Play("gem_reward");
    }

    public void CloseRewardPanel()
    {
        rewardUI.alpha = 0;
        rewardUI.interactable = false;
        rewardUI.blocksRaycasts = false;

    }

    public void BallAliveAdded(GameObject newBall)
    {
        aliveBalls.Add(newBall);
    }

    public void SpawnBall()
    {
        //Vector3 ballHolderPosition = GameObject.FindGameObjectWithTag ("BallHolder").transform.position;
        Vector3 ballHolderPosition = breaker.ballHolder.transform.position;
        ballHolderPosition.y = 0.75f;

        GameObject newBall = Instantiate(ballPrefab, ballHolderPosition, Quaternion.identity);
        aliveBalls.Add(newBall);
        ballsLeft -= 1;
        UpdateBallsUI();
    }

    public void LostBall(GameObject ball)
    {
        AudioManager.instance.Play("GameOver");
        aliveBalls.Remove(ball);
        if (aliveBalls.Count < 1)
        {
            AddScore(-Mathf.Min(score, 10));
            if (ballsLeft > 0)
            {
                ResetPlayer();
            }
            else
            {
                StartCoroutine(GameOver());
            }
        }
    }

    public void ResetPlayer()
    {
        var powerups = FindObjectsOfType<Powerup>();
        foreach (var powerup in powerups)
        {
            Destroy(powerup.gameObject);
        }
        
        var spells = FindObjectsOfType<SpellObject>();
        foreach(var spell in spells)
        {
            Destroy(spell.gameObject);
        }

        var breaker = FindObjectOfType<Breaker>();
        var newPos = breaker.transform.position;
        newPos.x = 0;
        breaker.transform.position = newPos;
        //btnShoot.gameObject.SetActive(true);

        breaker.ChangeState(Breaker.BreakerState.Aim);
        
        SpawnBall();        
        //GenerateSpells();
    }

    public void UpdateBallsUI()
    {
        foreach (Transform child in ballUiHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ballsLeft; i++)
        {
            Instantiate(ballUiPrefab, ballUiHolder);
        }

        //if (txtBalls != null)
        //{
        //	txtBalls.text ="Balls: " + ballsLeft.ToString();
        //}	
    }

    public void UpdateCookiesUI()
    {
        if (txtCookies != null)
        {
            txtCookies.text = StageManager.instance.playerData.Cookies.ToString();
        }
    }

    public void UpdateScoreUI()
    {
        if (txtScore != null)
        {
            txtScore.text = score.ToString();
            txtScore.GetComponent<Animator>().SetTrigger("Collect");
        }
    }



    void Awake()
    {
        pause_Btn.alpha = 1;
        pause_Btn.blocksRaycasts = true;
        pause_Btn.interactable = true;
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }



    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void PauseMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.alpha = 1;
            pauseMenu.interactable = true;
            pauseMenu.blocksRaycasts = true;
            pauseBtnImage.sprite = resumeSprite;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.alpha = 0;
            pauseMenu.interactable = false;
            pauseMenu.blocksRaycasts = false;
            pauseBtnImage.sprite = pauseSprite;
            options_Menu.alpha = 0;
            options_Menu.interactable = false;
            options_Menu.blocksRaycasts = false;
            Time.timeScale = 1f;
        }
    }

    public void Return()
    {
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        Time.timeScale = 1f;
    }

    public void OptionsMenuOpen()
    {
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        options_Menu.alpha = 1;
        options_Menu.interactable = true;
        options_Menu.blocksRaycasts = true;
    }

    public void OptionsMenuClose()
    {
        options_Menu.alpha = 0;
        options_Menu.interactable = false;
        options_Menu.blocksRaycasts = false;
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;
    }

    public void ShowShopMenu()
    {
        shopUI.alpha = 1;
        shopUI.interactable = true;
        shopUI.blocksRaycasts = true;
        shopUI.GetComponent<ShopMenu>().CreateMenu();
    }

    public void HideShopMenu()
    {
        shopUI.alpha = 0;
        shopUI.interactable = false;
        shopUI.blocksRaycasts = false;
    }

    public void SetQuality(int qualityIndex)
    {
        int qualityLevel = 0;
        switch (qualityIndex)
        {
            case 0:
                qualityLevel = 0;
                break;
            case 1:
                qualityLevel = 3;
                break;
            case 2:
                qualityLevel = 5;
                break;
            default:
                qualityLevel = 0;
                break;
        }
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    //#region spells
    //public void GenerateSpells()
    //{       
    //    SpellPanel.GetComponent<CanvasGroup>().alpha = 1;
    //    SpellPanel.GetComponent<CanvasGroup>().interactable = true;
    //    SpellPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    //    DestroySpellUI();
    //    //Unlocked spells
    //    foreach (var spell in StageManager.instance.spells)
    //    {
    //        var unlockedSpellCharges = StageManager.instance.GetSpellCharges(spell);
    //        var isUnlocked = StageManager.instance.IsSpellUnlocked(spell);

    //        var spellBtn = Instantiate(SpellUIPrefab, SpellPanel);
    //        spellBtn.GetComponent<Button>().interactable = isUnlocked;
    //        spellBtn.GetComponentInChildren<TextMeshProUGUI>().text = isUnlocked ? unlockedSpellCharges.ToString() : "";
    //        spellBtn.Find("Icon").GetComponent<Image>().sprite = isUnlocked ? spell.Icon : lockSprite;

    //        ColorBlock greyBlock = spellBtn.GetComponent<Button>().colors;
    //        greyBlock.normalColor = Color.gray;
    //        greyBlock.highlightedColor = Color.gray;
    //        greyBlock.pressedColor = Color.gray;
    //        spellBtn.GetComponent<Button>().colors = greyBlock;
    //        var spellName = spell.Name;
    //        spellBtn.GetComponent<Button>().onClick.AddListener(delegate
    //        {

    //            SelectSpell(spellName);
    //            ColorBlock colorBlock = spellBtn.GetComponent<Button>().colors;
    //            if (selectedSpell == spellName)
    //            {
    //                colorBlock.normalColor = Color.magenta;
    //                colorBlock.highlightedColor = Color.magenta;
    //                colorBlock.pressedColor = Color.magenta;
    //                spellBtn.GetComponent<Button>().colors = colorBlock;
    //            }
    //            else
    //            {                    
    //                colorBlock.normalColor = Color.gray;
    //                colorBlock.highlightedColor = Color.gray;
    //                colorBlock.pressedColor = Color.gray;
    //                spellBtn.GetComponent<Button>().colors = colorBlock;
    //            }
    //        });

    //    }

    //}

    //public void HideSpells()
    //{
    //    SpellPanel.GetComponent<CanvasGroup>().alpha = 0;
    //    SpellPanel.GetComponent<CanvasGroup>().interactable = false;
    //    SpellPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    //}

    //public void DestroySpellUI()
    //{
    //    foreach (Transform child in SpellPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }



    //}

    //public void ActivateSpell()
    //{
    //    if (selectedSpell == "") return;
    //    switch (selectedSpell)
    //    {
    //        case "Slo-mo Shield":
    //            StageManager.instance.UseSpell(selectedSpell);
    //            Instantiate(sloMoPrefab, sloMoSpawn, Quaternion.Euler(-90f,0,0));
    //            break;
    //    }


    //}

    //public void SelectSpell(string spellName)
    //{        
    //    if (selectedSpell != "" && selectedSpell == spellName)
    //    {
    //        selectedSpell = "";
    //    }
    //    else
    //    {            
    //        selectedSpell = spellName;
    //    }
    //}
    //#endregion

}

