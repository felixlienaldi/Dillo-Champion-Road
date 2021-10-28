using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobInter_Gameobject : MonoBehaviour
{
    private InterstitialAd interstitial;
    public string m_AppID = "ca-app-pub-3940256099942544/1033173712";
    public void Start() {
        #if UNITY_ANDROID
        RequestInterstitial();
#endif
    }

    private void RequestInterstitial() {
#if UNITY_ANDROID
        string adUnitId =m_AppID;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void f_ShowAd() {
        #if UNITY_ANDROID
        if (!Player_Manager.m_Instance.m_BoughAds) {
            this.interstitial.Show();
        }
#endif
    }
}
