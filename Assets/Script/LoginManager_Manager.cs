using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static LoginManager_Manager m_Instance;
    //===== STRUCT =====
    [System.Serializable]
    public class c_LoginData
    {
        public string SessionTicket;
        public string PlayFabId;
        public string NewlyCreated;
        public c_UserSetting SettingsForUser;
        public string LastLoginTime;
        public c_EntityToken EntityToken;
        public c_TreatmentAssignment TreatmentAssignment;
    }
    [System.Serializable]
    public class c_EntityToken
    {
        public string EntityToken;
        public string TokenExpiration;
        public c_EntityKey Entity;
    }
    [System.Serializable]
    public class c_EntityKey
    {
        public string Id;
        public string Type;
        public string TypeString;
    }
    [System.Serializable]
    public class c_UserSetting
    {
        public bool NeedsAttribution;
        public bool GatherDeviceInfo;
        public bool GatherFocusInfo;
    }
    [System.Serializable]
    public class c_TreatmentAssignment
    {
        public string[] Variants;
        public string[] Variables;
    }

    //===== PUBLIC =====
    public c_LoginData m_LoginData;
    public bool m_Google = false;
    public bool m_GuestLoggedIn;
    public bool m_IsLinked;
    //===== PRIVATES =====
    Firebase.FirebaseApp app;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        Debug.Log(ReturnAndroidID());
        f_CheckFirebase();
        UIManager_Manager.m_Instance.f_LoadinStart();
        f_InitializePlayGamesConfig();
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_CheckFirebase() {
#if UNITY_ANDROID
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
#endif
    }
    /// <summary>
    /// Method that will be called after Guest Login Request, resulted in success
    /// </summary>
    /// <param name="p_Result">Result details from the request</param>
    private void f_OnLoginSuccess(LoginResult p_Result) {
        m_LoginData = JsonUtility.FromJson<c_LoginData>(p_Result.ToJson());
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = m_LoginData.PlayFabId }, f_OnAccountInfo, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method for Requesting PlayFabClientAPI a Guest Login
    /// </summary>
    public void f_GuestLoginRequest() {
#if UNITY_ANDROID
        LoginWithAndroidDeviceIDRequest RequestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(RequestAndroid, f_OnLoginSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
#elif UNITY_STANDALONE_WIN
        LoginWithCustomIDRequest t_Req = new LoginWithCustomIDRequest {
            CustomId = "Testing",
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(t_Req, f_OnLoginSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
#endif
    }

    /// <summary>
    /// Method for requesting unlink Android Device ID to playfab
    /// </summary>
    public void f_UnlinkGuestRequest() {
        PlayFabClientAPI.UnlinkAndroidDeviceID(new UnlinkAndroidDeviceIDRequest {
            AndroidDeviceId = ReturnAndroidID(),
        }, OnUnlinkGuestSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method that will be called after Unlink Android ID from playfab resulted in success
    /// </summary>
    /// <param name="p_Result">Result details from request</param>
    public void OnUnlinkGuestSuccess(UnlinkAndroidDeviceIDResult p_Result) {
       Debug.Log("Success Unlink Guest");
    }

    public static string ReturnAndroidID() {
        return SystemInfo.deviceUniqueIdentifier;
    }
#if UNITY_ANDROID
#region GOOGLE
    /// <summary>
    /// Method for Initialize Google Play Games Config
    /// </summary>
    public void f_InitializePlayGamesConfig() {
        // The following grants profile access to the Google Play Games SDK.
        // Note: If you also want to capture the player's Google email, be sure to add
        // .RequestEmail() to the PlayGamesClientConfiguration
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .AddOauthScope("profile")
        .RequestServerAuthCode(false)
        .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        f_GoogleSignInRequest();
    }

    /// <summary>
    /// Method for Login to Google Request, will be put in button google sign in
    /// </summary>
    public void f_GoogleSignInRequest() {
        Debug.Log("Google");
        Social.localUser.Authenticate((p_Success) => {
            if (p_Success) {
                string t_ServerAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                m_Google = true;
                f_LoginWithGoogle(t_ServerAuthCode);
            }
            else {
                m_Google = false;
                f_GuestLoginRequest();
            }

        });
    }

    /// <summary>
    /// Method for Login Playfab with google request
    /// </summary>
    /// <param name="p_ServerAuthCode">Server Authorization Code from google</param>
    public void f_LoginWithGoogle(string p_ServerAuthCode) {
        PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest() {
            TitleId = PlayFabSettings.TitleId,
            ServerAuthCode = p_ServerAuthCode,
            CreateAccount = true
        }, f_OnLoginSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method for Link Playfab with google request
    /// </summary>
    /// <param name="p_ServerAuthCode">Server Authorization Code from google</param>
    public void f_LinkWithGoogle(string p_ServerAuthCode) {
        PlayFabClientAPI.LinkGoogleAccount(new LinkGoogleAccountRequest() {
            ForceLink = true,
            ServerAuthCode = p_ServerAuthCode,
        }, OnPlayfabLinkGoogleSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method that will be called after link playfab with google request return true / succeed
    /// </summary>
    /// <param name="p_Result">result details from request</param>
    public void OnPlayfabLinkGoogleSuccess(LinkGoogleAccountResult p_Result) {
        m_IsLinked = true;
        f_UnlinkGuestRequest();
    }

#endregion
#endif
    public void f_OnAccountInfo(GetAccountInfoResult p_Result) {
        if (p_Result.AccountInfo.TitleInfo.Created == p_Result.AccountInfo.TitleInfo.LastLogin) {
#if UNITY_ANDROID
            string t_Name;
            if (m_Google) t_Name = PlayGamesPlatform.Instance.GetUserDisplayName();
            else t_Name = m_LoginData.PlayFabId;
#endif
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = t_Name }, f_OnDisplayNameSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
        }
        else f_CallAPI(false);
    }

    public void f_CallAPI(bool p_New) {
        if (p_New) {
            PlayerStatistic_Manager.m_Instance.f_NewPlayer();
        }
        else {
            PlayerStatistic_Manager.m_Instance.f_GetPlayerStatistic();

        }

        PushNotification_GameObject.m_Instance.f_RegisterForPush();
        CurrencyManager_Manager.m_Instance.f_GetCurrency();
        LeaderboardManager_Manager.m_Instance.f_GetLeaderBoard();
        PlayerData_Manager.m_Instance.f_GetPlayerData();
        //LeaderboardManager_Manager.m_Instance.f_GetPlayerLeaderBoard();
        Timer_Manager.m_Instance.f_GetTimeServer();
    }

    public void f_OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult p_Result) {
        Player_Manager.m_Instance.m_Names = p_Result.DisplayName;        
        f_CallAPI(true);
    }
}
