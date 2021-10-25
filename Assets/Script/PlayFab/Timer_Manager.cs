using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class Timer_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Timer_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public DateTime m_Time = new DateTime();
    public TimeSpan m_Span;
    public TextMeshProUGUI m_Timer1;
    public TextMeshProUGUI m_Timer2;
    public bool m_TrackTimer = false;
    //===== PRIVATES =====
    DateTime m_NextTime = new DateTime();
    TimeSpan t_Span;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        if (m_NextTime.Subtract(DateTime.Now).TotalSeconds <= 0) {
            if (m_TrackTimer) {
                f_GetTimeServer();
                m_TrackTimer = false;
            }
        }
        else {
            m_Span = m_NextTime.Subtract(DateTime.Now);
            m_Timer1.text = string.Format("{00:00}:{01:00}:{02:00}",m_Span.Hours, m_Span.Minutes, m_Span.Seconds);
            m_Timer2.text = string.Format("{00:00}:{01:00}:{02:00}", m_Span.Hours, m_Span.Minutes, m_Span.Seconds);
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_GetTimeServer() {
        PlayFabClientAPI.GetTime(new GetTimeRequest(), f_OnGetTimeSuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_OnGetTimeSuccess(GetTimeResult p_Result) {
        m_Time = p_Result.Time;
        if (m_Time.Hour >= 0 && m_Time.Hour <= 5) {
            m_NextTime = new DateTime(m_Time.Year,m_Time.Month,m_Time.Day,6,0,0);
            m_NextTime = DateTime.Now.AddSeconds(m_NextTime.Subtract(m_Time).TotalSeconds);
            Fragment_Manager.m_Instance.m_DailyShopFrags = 0;
        }
        else if (m_Time.Hour >= 6 && m_Time.Hour <= 11) {
            m_NextTime = new DateTime(m_Time.Year, m_Time.Month, m_Time.Day, 12, 0, 0);
            m_NextTime = DateTime.Now.AddSeconds(m_NextTime.Subtract(m_Time).TotalSeconds);
            Fragment_Manager.m_Instance.m_DailyShopFrags = 1;
        }
        else if (m_Time.Hour >= 12 && m_Time.Hour <= 17) {
            m_NextTime = new DateTime(m_Time.Year, m_Time.Month, m_Time.Day, 18, 0, 0);
            m_NextTime = DateTime.Now.AddSeconds(m_NextTime.Subtract(m_Time).TotalSeconds);
            Fragment_Manager.m_Instance.m_DailyShopFrags = 2;
        }
        else if (m_Time.Hour >= 18 && m_Time.Hour <= 23) {
            m_NextTime = new DateTime(m_Time.Year, m_Time.Month, m_Time.AddDays(1).Day, 0, 0, 0);
            m_NextTime = DateTime.Now.AddSeconds(m_NextTime.Subtract(m_Time).TotalSeconds);
            Fragment_Manager.m_Instance.m_DailyShopFrags = 3;
        }
        m_Span = m_NextTime.Subtract(DateTime.Now);
        PlayerStatistic_Manager.m_Instance.f_GetPlayerStatistic();
        m_TrackTimer = true;
    }
}
