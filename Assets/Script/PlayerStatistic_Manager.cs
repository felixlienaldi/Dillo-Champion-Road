using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerStatistic_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PlayerStatistic_Manager m_Instance;
    //===== STRUCT =====

    [System.Serializable]
    public class c_ListStatistic
    {
        public List<StatisticUpdate> Statistics;
    }

    public c_ListStatistic m_ListStatistic;
    //===== PUBLIC =====
    //===== PRIVATES =====
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    
    void Awake(){
        m_Instance = this;
    }

    void Start(){

    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    
    public void f_UpdatePlayerStatistics() {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = m_ListStatistic.Statistics
        }, f_OnUpdateStatisticSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_OnUpdateStatisticSuccess(UpdatePlayerStatisticsResult p_Result) {
        f_GetPlayerStatistic();
    }

    public void f_GetPlayerStatistic() {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest {
        }, f_OnGetStatisticsSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_OnGetStatisticsSuccess(GetPlayerStatisticsResult p_Result) {
        m_ListStatistic = JsonUtility.FromJson<c_ListStatistic>(p_Result.ToJson());
        
    }

}
