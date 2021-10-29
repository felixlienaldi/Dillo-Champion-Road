using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyMissionButton_Gameobject : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====

    //===== PUBLIC =====
    public GameObject m_ClaimButton;
    public GameObject m_DefaultButton;
    public Button m_Button;
    public int m_Id = 0;
    public Image m_Bar;
    public GameObject m_Checkmark;
    public TextMeshProUGUI m_RequiredAmount;
    public TextMeshProUGUI m_CurrentAmount;
    public TextMeshProUGUI m_Line;
    public TextMeshProUGUI m_AcceptText;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Start() {
        m_Button = m_ClaimButton.GetComponent<Button>();
    }

    void Update() {
        f_CheckValidToClaim();
        if (m_Id == 1) {
            m_Bar.fillAmount = (float)DailyMission_Manager.m_Instance.m_CurrentPlayMatch / DailyMission_Manager.m_Instance.m_RequiredPlayMatch;
            m_CurrentAmount.text = DailyMission_Manager.m_Instance.m_CurrentPlayMatch.ToString();
            m_RequiredAmount.text = DailyMission_Manager.m_Instance.m_RequiredPlayMatch.ToString();
        }
        else if (m_Id == 2) {
            m_Bar.fillAmount = (float)DailyMission_Manager.m_Instance.m_CurrentDestroyedEnemy / DailyMission_Manager.m_Instance.m_RequiredDestroyedEnemy;
            m_CurrentAmount.text = DailyMission_Manager.m_Instance.m_CurrentDestroyedEnemy.ToString();
            m_RequiredAmount.text = DailyMission_Manager.m_Instance.m_RequiredDestroyedEnemy.ToString();
        }
        else if (m_Id == 3) {
            m_Bar.fillAmount = (float)DailyMission_Manager.m_Instance.m_CurrentCombo / DailyMission_Manager.m_Instance.m_RequiredCombo;
            m_CurrentAmount.text = DailyMission_Manager.m_Instance.m_CurrentCombo.ToString();
            m_RequiredAmount.text = DailyMission_Manager.m_Instance.m_RequiredCombo.ToString();
        }
        else if (m_Id == 4) {
            if (DailyMission_Manager.m_Instance.m_EnemyID %2 == 0) {
                m_Line.text = "destroy 100x normal enemies";
            }
            else {
                m_Line.text = "destroy 100x inverted enemies";
            }
            m_Bar.fillAmount = (float) DailyMission_Manager.m_Instance.m_CurrentEnemy / DailyMission_Manager.m_Instance.m_RequiredEnemy;
            m_CurrentAmount.text = DailyMission_Manager.m_Instance.m_CurrentEnemy.ToString();
            m_RequiredAmount.text = DailyMission_Manager.m_Instance.m_RequiredEnemy.ToString();
        }
        else if (m_Id == 5) {
            m_Bar.fillAmount = (float)DailyMission_Manager.m_Instance.m_MissionComplete / 4f;
            m_CurrentAmount.text = DailyMission_Manager.m_Instance.m_MissionComplete.ToString();
            m_RequiredAmount.text = "4";
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_CheckValidToClaim() {
        m_ClaimButton.SetActive(DailyMission_Manager.m_Instance.f_CheckValid(m_Id) || DailyMission_Manager.m_Instance.f_Checkmark(m_Id));
        m_Button.interactable = DailyMission_Manager.m_Instance.f_CheckValid(m_Id);
        m_DefaultButton.SetActive(!DailyMission_Manager.m_Instance.f_CheckValid(m_Id) && !DailyMission_Manager.m_Instance.f_Checkmark(m_Id));
        if (DailyMission_Manager.m_Instance.f_Checkmark(m_Id)) {
            m_Checkmark.gameObject.SetActive(true);
            m_AcceptText.color = Color.gray;
        }
        else {
            m_Checkmark.gameObject.SetActive(false);
            m_AcceptText.color = Color.white;
        }
    }

    public void f_Claim() {
        if (m_Id == 1) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", 10);
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyMission1", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(0,10);
        }
        else if (m_Id == 2) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", 10);
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyMission2", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(0,10);
        }
        else if (m_Id == 3) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", 10);
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyMission3", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(0,10);
        }
        else if (m_Id == 4) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", 10);
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyMission4", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(0,10);
        }
        else if (m_Id == 5) {
            Fragment_Manager.m_Instance.f_CalculateGacha();
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyMission5", 1);
        }

        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyProgress5", DailyMission_Manager.m_Instance.m_MissionComplete);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
    }
}
