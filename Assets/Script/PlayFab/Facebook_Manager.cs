using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class Facebook_Manager : MonoBehaviour {

    private void Awake() {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void InitCallback() {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            f_LogAppEvent();
        }
        else {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void f_LogAppEvent() {
        var tutParams = new Dictionary<string, object>();
        tutParams[AppEventParameterName.ContentID] = "sentFriendRequest";
        tutParams[AppEventParameterName.Description] = "SDK  Activated";
        tutParams[AppEventParameterName.Success] = "1";

        FB.LogAppEvent(
            AppEventName.CompletedTutorial,
            parameters: tutParams
        );
    }

}
