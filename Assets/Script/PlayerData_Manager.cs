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

    public void f_UpdatePlayerAvatarList(string p_AvatarList) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest {
            Data = new Dictionary<string, string> {
                {m_AvatarListKey, p_AvatarList},
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
    }
   
}