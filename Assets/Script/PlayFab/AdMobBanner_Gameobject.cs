using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;
public class AdMobBanner_Gameobject : MonoBehaviour {

    public static AdMobBanner_Gameobject m_Instance;

    public string m_AppId = "ca-app-pub-3940256099942544/6300978111";
    private BannerView bannerView;
    public bool m_Shown = false;
    private void Awake() {
        m_Instance = this;
    }

    public void Start() {
#if UNITY_ANDROID
        // Initialize the Google Mobile Ads SDK.
       MobileAds.Initialize(initStatus => { });

       RequestBanner();
#endif
    }

    private void RequestBanner() {
#if UNITY_ANDROID
        string adUnitId = m_AppId;
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerView.Hide();
    }

    public void f_ShowBanner() {
#if UNITY_ANDROID
        if (!m_Shown) {
            bannerView.Show();
            m_Shown = true;
        }
#endif
    }

    public void f_HideBanner() {
#if UNITY_ANDROID
        if (m_Shown) {
            bannerView.Hide();
            m_Shown = false;
        }
#endif
    }
}
