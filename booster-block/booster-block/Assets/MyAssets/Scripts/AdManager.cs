using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

    public static AdManager instance;
    string gameId = "2917948";
    public string placementId = "bottomBanner";
    public bool testMode = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

        }
        DontDestroyOnLoad(gameObject);
    }


    // bool testMode = true;

    void Start()
    {
        Advertisement.Initialize(gameId);
        //Advertisement.Banner.Load(placementId);
        //   StartCoroutine(ShowBannerWhenReady());
    }

    public void ShowBannerAd()
    {
        //StartCoroutine(ShowBannerWhenReady());
        Advertisement.Show(placementId);
    }

    //IEnumerator ShowBannerWhenReady()
    //{
        
    //    while (!Advertisement.IsReady())
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    Advertisement.Banner.Show(placementId);
    //}
}
