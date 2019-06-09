using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Toggle backgroundMusicToggle;
    public Dropdown graphicsDropdown;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetQuality(int qualityIndex)
    {
        int qualityLevel = 0;
        switch (qualityIndex)
        {
            case 0:
                qualityLevel = 2;
                break;
            case 1:
                qualityLevel = 1;
                break;
            case 2:
                qualityLevel = 0;
                break;
            default:
                qualityLevel = 0;
                break;
        }
        QualitySettings.SetQualityLevel(qualityLevel);
        StageManager.instance.SetQualityLevel(qualityLevel);
    }

    public int GetQualityIndex(int qualityLevel)
    {
        switch(qualityLevel)
        {
            case 2:
                return 0;
            case 1:
                return 1;
            case 0:
                return 2;
            default:
                return 0;
        }
    }

    public void SetBackgroundMusic(bool value)
    {
        StageManager.instance.SetBackgroundMusic(value);
    }

    public void ShowMenu()
    {
        backgroundMusicToggle.isOn = StageManager.instance.playerData.BackgroundMusic;
        //Debug.Log(QualitySettings.GetQualityLevel());
        graphicsDropdown.value = GetQualityIndex(StageManager.instance.playerData.QualityLevel);
        //graphicsDropdown.set
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}
