using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class PlayerData_Manager : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PlayerData_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    [System.Serializable]
    public class c_Data : SerializableDictionary<string, c_DataDetails> { }

    [System.Serializable]
    public class c_PlayerDataList {
        public c_Data Data = new c_Data();
        public string DataVersion;
    }

    [System.Serializable]
    public class c_DataDetails {
        public string Value;
        public string LastUpdated;
        public string Permission;
    }

    public c_PlayerDataList m_PlayerDataList;
    //===== PRIVATES =====
    const string m_ShirtKey = "CLOTHES";
    const string m_PantKey = "PANTS";
    const string m_AvatarListKey = "AVATARLIST";
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    private void Awake() {
        m_Instance = this;
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_UpdatePlayerAvatarData(int p_ShirtID, int p_PantID) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest {
            Data = new Dictionary<string, string> {
                {m_ShirtKey, p_ShirtID.ToString()},
                {m_PantKey, p_PantID.ToString()}
            },
            Permission = UserDataPermission.Public
        }, f_OnUpdatePlayerDataSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_UpdatePlayerAvatarList(string p_AvatarKey, string p_AvatarList) {
        UIManager_Manager.m_Instance.f_LoadinStart();
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest {
            Data = new Dictionary<string, string> {
                {p_AvatarKey, p_AvatarList},
            },
            Permission = UserDataPermission.Public
        }, f_OnUpdatePlayerDataSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_GetPlayerData() {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = LoginManager_Manager.m_Instance.m_LoginData.PlayFabId,
            Keys = null,
        }, f_OnGetPlayerDataSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_OnUpdatePlayerDataSuccess(UpdateUserDataResult p_Result) {
        f_GetPlayerData();
    }

    public void f_OnGetPlayerDataSuccess(GetUserDataResult p_Result) {
        m_PlayerDataList = JsonConvert.DeserializeObject<c_PlayerDataList>(p_Result.ToJson());
        GameManager_Manager.m_Instance.m_ListPotion.Clear();
        if (m_PlayerDataList.Data.TryGetValue("ACCURACY", out c_DataDetails t_AccuracyKey)) {
            PowerupUI_Manager.m_Instance.f_LoadDataPotion("ACCURACY", t_AccuracyKey.Value);
        }

        if (m_PlayerDataList.Data.TryGetValue("BARRIER", out c_DataDetails t_BarrierKey)) {
            PowerupUI_Manager.m_Instance.f_LoadDataPotion("BARRIER", t_BarrierKey.Value);
        }
        if (m_PlayerDataList.Data.TryGetValue("REVIVE", out c_DataDetails t_ReviveKey)) {
            PowerupUI_Manager.m_Instance.f_LoadDataPotion("REVIVE", t_ReviveKey.Value);
        }
        if (m_PlayerDataList.Data.TryGetValue("FEVERGAIN", out c_DataDetails t_FeverGainKey)) {
            PowerupUI_Manager.m_Instance.f_LoadDataPotion("FEVERGAIN", t_FeverGainKey.Value);
        }
        if (m_PlayerDataList.Data.TryGetValue("EquippedSkin", out c_DataDetails t_EqSkinKey)) {
            Wardobe_Manager.m_Instance.f_LoadEquipedSkinData(t_EqSkinKey.Value);
        }
        if (m_PlayerDataList.Data.TryGetValue("SkinList", out c_DataDetails t_ListSkinKey)) {
            Wardobe_Manager.m_Instance.f_LoadSkinData(t_ListSkinKey.Value);
        }

        if (m_PlayerDataList.Data.TryGetValue("Ads", out c_DataDetails t_AdsKey)) {
            if (t_AdsKey.Value == "0") {
                Player_Manager.m_Instance.m_BoughAds = false;
                AdMobBanner_Gameobject.m_Instance.f_ShowBanner();
            }
            else {
                Player_Manager.m_Instance.m_BoughAds = true;
                AdMobBanner_Gameobject.m_Instance.f_HideBanner();
            }

        }
        else {
            Player_Manager.m_Instance.m_BoughAds = false;
            AdMobBanner_Gameobject.m_Instance.f_ShowBanner();
        }
        GameManager_Manager.m_Instance.f_ApplyPotion();
        UIManager_Manager.m_Instance.f_LoadingFinish();
    }
}