using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

[System.Serializable]
public class c_Event : UnityEvent { }

public class AdMobRewardedInter_Gameobject : MonoBehaviour {
    private RewardedAd rewardedAd;
    public string m_AdUnitID = "ca-app-pub-3940256099942544/5224354917";
    public c_Event m_OnCloseSuccess;
    public c_Event m_OnCloseFail;
    bool m_SuccessWatchAds = false;
    bool m_Closed = false;
    public void Start() {
#if UNITY_ANDROID
        f_LoadAds();
#endif
    }

    public void Update() {
        if (m_Closed) {
            m_Closed = false;
            f_CheckAd();
        }
    }

    public void f_ShowAd() {
#if UNITY_ANDROID
        m_SuccessWatchAds = false;
        this.rewardedAd.Show();
#else
        this.m_OnFinishWatchingAds?.Invoke();
        this.m_OnClose?.Invoke();
#endif
    }

    public void f_LoadAds() {
#if UNITY_ANDROID
        string adUnitId = m_AdUnitID;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
        string adUnitId = "unexpected_platform";
#endif
        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        f_CheckAd();
        this.f_LoadAds();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        this.f_LoadAds();
        m_Closed = true;
    }

    public void HandleUserEarnedReward(object sender, Reward args) {
        m_SuccessWatchAds = true; 
    }

    public void f_CheckAd() {
        if (m_SuccessWatchAds) {
            this.m_OnCloseSuccess?.Invoke();
        }
        else {
            this.m_OnCloseFail?.Invoke();
        }
    }

}
