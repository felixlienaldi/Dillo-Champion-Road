using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyMission_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static DailyMission_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    [Header("Mission 1 (Play X Match)")]
    public int m_RequiredPlayMatch = 5;
    public int m_CurrentPlayMatch = 0;
    public int m_DMToken1 = 0;

    [Header("Mission 2 (Destroy X Enemy)")]
    public int m_RequiredDestroyedEnemy = 500;
    public int m_CurrentDestroyedEnemy = 0;
    public int m_DMToken2 = 0;

    [Header("Mission 3 (Achieve X Combo)")]
    public int m_RequiredCombo = 250;
    public int m_CurrentCombo = 0;
    public int m_DMToken3 = 0;

    [Header("Mission 4 (Destroy X Enemy)")]
    public int m_RequiredEnemy = 100;
    public int m_CurrentEnemy = 0;
    public int m_EnemyID = 0;
    public int m_DMToken4 = 0;

    [Header("Mission 5 (Complete All Daily)")]
    public int m_MissionComplete = 0;
    public int m_DMToken5 = 0;
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
    public bool f_CheckValid(int p_RewardToken) {
        if (p_RewardToken == 1) {
            if (m_RequiredPlayMatch == m_CurrentPlayMatch && m_DMToken1 == 0) return true;
            else return false;
        }
        else if (p_RewardToken == 2) {
            if (m_CurrentDestroyedEnemy == m_RequiredDestroyedEnemy && m_DMToken2 == 0) return true;
            else return false;
        }
        else if (p_RewardToken == 3) {
            if (m_CurrentCombo == m_RequiredCombo && m_DMToken3 == 0) return true;
            else return false;
        }
        else if (p_RewardToken == 4) {
            if (m_CurrentEnemy == m_RequiredEnemy && m_DMToken4 == 0) return true;
            else return false;
        }
        else {
            if (m_MissionComplete == 4 && m_DMToken5 == 0) return true;
            else return false;
        }
    }

    public void f_ResetToken() {
        m_MissionComplete = 0;
    }

    public void f_RegisterToken(int p_TokenCode,int p_Value) {
        if (p_TokenCode == 1) {
            m_DMToken1 = p_Value;
            if (p_Value == 1) m_MissionComplete++;
        }
        else if (p_TokenCode == 2) {
            Debug.Log(p_Value);
            m_DMToken2 = p_Value;
            if (p_Value == 1) m_MissionComplete++;
        }
        else if (p_TokenCode == 3) {
            m_DMToken3 = p_Value;
            if (p_Value == 1) m_MissionComplete++;
        }
        else if (p_TokenCode == 4) {
            m_DMToken4 = p_Value;
            if (p_Value == 1) m_MissionComplete++;
        }
        else {
            m_DMToken5 = p_Value;
        } 
    }

    public bool f_Checkmark(int p_RewardToken) {
        if (p_RewardToken == 1 && m_DMToken1 == 1) return true;
        else if (p_RewardToken == 2 && m_DMToken2 == 1) return true;
        else if (p_RewardToken == 3 && m_DMToken3 == 1) return true;
        else if (p_RewardToken == 4 && m_DMToken4 == 1) return true;
        else if (p_RewardToken == 5 && m_DMToken5 == 1) return true;
        else return false;
    }

    public void f_AddCurrentMatch() {
        m_CurrentPlayMatch++;
        if (m_CurrentPlayMatch >= m_RequiredPlayMatch) {
            m_CurrentPlayMatch = m_RequiredPlayMatch;
        }
    }

    public void f_AddDestroyedEnemy(int p_Amount) {
        m_CurrentDestroyedEnemy += p_Amount;
        if (m_CurrentDestroyedEnemy >= m_RequiredDestroyedEnemy) {
            m_CurrentDestroyedEnemy = m_RequiredDestroyedEnemy;    
        }
    }

    public void f_CompareCombo(int p_ComboAmount) {
        if (p_ComboAmount > m_CurrentCombo) {
            m_CurrentCombo = p_ComboAmount;
            if (m_CurrentCombo > m_RequiredCombo) m_CurrentCombo = m_RequiredCombo;
        }
    }

    public void f_AddCertainEnemy() {
        m_CurrentEnemy++;
        if (m_CurrentEnemy >= m_RequiredEnemy) {
            m_CurrentEnemy = m_RequiredEnemy;
        }
    }

    public void f_Endgame() {
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress1", m_CurrentPlayMatch);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress2", m_CurrentDestroyedEnemy);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress3", m_CurrentCombo);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress4", m_CurrentEnemy);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress5", m_MissionComplete);
    }
}
