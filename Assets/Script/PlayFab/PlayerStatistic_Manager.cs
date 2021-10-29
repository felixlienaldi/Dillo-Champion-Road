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
        UIManager_Manager.m_Instance.f_LoadinStart();
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
        DailyMission_Manager.m_Instance.f_ResetToken();
        foreach (StatisticUpdate t_Stats in m_ListStatistic.Statistics) {
            switch (t_Stats.StatisticName) {
                case "DailyMission1":
                    DailyMission_Manager.m_Instance.f_RegisterToken(1, t_Stats.Value);
                    break;
                case "DailyMission2":
                    DailyMission_Manager.m_Instance.f_RegisterToken(2, t_Stats.Value);
                    break;
                case "DailyMission3":
                    DailyMission_Manager.m_Instance.f_RegisterToken(3, t_Stats.Value);
                    break;
                case "DailyMission4":
                    DailyMission_Manager.m_Instance.f_RegisterToken(4, t_Stats.Value);
                    break;
                case "DailyMission5":
                    DailyMission_Manager.m_Instance.f_RegisterToken(5, t_Stats.Value);
                    break;
                case "DailyProgress1":
                    DailyMission_Manager.m_Instance.m_CurrentPlayMatch = t_Stats.Value;
                    break;
                case "DailyProgress2":
                    DailyMission_Manager.m_Instance.m_CurrentDestroyedEnemy = t_Stats.Value;
                    break;
                case "DailyProgress3":
                    DailyMission_Manager.m_Instance.m_CurrentCombo = t_Stats.Value;
                    break;
                case "DailyProgress4":
                    DailyMission_Manager.m_Instance.m_CurrentEnemy = t_Stats.Value;
                    break;
                case "DailyProgress5":
                    //DailyMission_Manager.m_Instance.m_MissionComplete = t_Stats.Value;
                    break;
                case "GachaTicket":
                    Fragment_Manager.m_Instance.m_GachaTicket = t_Stats.Value;
                    break;
                case "Highscore":
                    Player_Manager.m_Instance.m_HighScore = t_Stats.Value;
                    break;
                case "ShopBought":
                    if (t_Stats.Value == 1) Fragment_Manager.m_Instance.m_DailyShopAvailable = false;
                    else Fragment_Manager.m_Instance.m_DailyShopAvailable = true;
                    break;
                case "FeverGain":
                    PowerupUI_Manager.m_Instance.m_FeverTimeBuff.m_Level = t_Stats.Value;
                    break;
                case "HpGainUp":
                    PowerupUI_Manager.m_Instance.m_HpGainUpBuff.m_Level = t_Stats.Value;
                    break;
                case "ScoreMultiplier":
                    PowerupUI_Manager.m_Instance.m_ScoreMultiplierBuff.m_Level = t_Stats.Value;
                    break;
                case "EnemyID":
                    DailyMission_Manager.m_Instance.m_EnemyID = (t_Stats.Value % 2 == 0 ? 0 : 1);
                    break;
                case "DailyBox":
                    Fragment_Manager.m_Instance.m_DailyBoxAvailable = (t_Stats.Value == 0 ? true : false);
                    break;
            }
        }
        UIManager_Manager.m_Instance.f_LoadingFinish();
        LeaderboardManager_Manager.m_Instance.f_GetPlayerLeaderBoard();
    }

    public void f_NewPlayer() {
        m_ListStatistic.Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {StatisticName = "DailyMission1",Value =0 },
            new StatisticUpdate {StatisticName = "DailyMission2",Value =0 },
            new StatisticUpdate {StatisticName = "DailyMission3",Value =0 },
            new StatisticUpdate {StatisticName = "DailyMission4",Value =0 },
            new StatisticUpdate {StatisticName = "DailyMission5",Value =0 },
            new StatisticUpdate {StatisticName = "DailyProgress1",Value =0 },
            new StatisticUpdate {StatisticName = "DailyProgress2",Value =0 },
            new StatisticUpdate {StatisticName = "DailyProgress3",Value =0 },
            new StatisticUpdate {StatisticName = "DailyProgress4",Value =0 },
            new StatisticUpdate {StatisticName = "DailyProgress5",Value =0 },
            new StatisticUpdate {StatisticName = "FeverGain",Value =0 },
            new StatisticUpdate {StatisticName = "GachaTicket",Value =5},
            new StatisticUpdate {StatisticName = "Highscore",Value =0},
            new StatisticUpdate {StatisticName = "HpGainUp",Value =0},
            new StatisticUpdate {StatisticName = "ScoreMultiplier",Value =0},
            new StatisticUpdate {StatisticName = "ShopBought",Value =0},
            new StatisticUpdate {StatisticName = "EnemyID",Value =0},
            new StatisticUpdate {StatisticName = "DailyBox",Value =0},
        };
        PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("Ads","0");
        f_UpdatePlayerStatistics();
    }

    public void f_UpdateStatistics(string p_Name, int p_Value) {
        for (int i = 0; i < m_ListStatistic.Statistics.Count; i++) {
            if (m_ListStatistic.Statistics[i].StatisticName == p_Name) {
 
                m_ListStatistic.Statistics[i].Value = p_Value;
            }
        }
    }
}
