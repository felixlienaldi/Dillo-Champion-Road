using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class Energy_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Energy_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public TextMeshProUGUI m_TimerText;
    public TextMeshProUGUI m_EnergyAmountText;
    public DateTime m_NextFreeTicket = new DateTime();
    public int m_EnergyAmount = 5;
    public int m_MaxEnergyAmont = 5;
    public bool m_IsStamCapped;
    public Button m_PlayButton;
    public Button m_RetryButton;
    //===== PRIVATES =====
    TimeSpan m_RechargeTime = new TimeSpan();
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        if (PlayFabClientAPI.IsClientLoggedIn()) {
            if (!m_IsStamCapped) {
                // リチャージ時間を迎えた場合はスタミナを再取得
                if (m_NextFreeTicket.Subtract(DateTime.Now).TotalSeconds <= 0) {
                    if (m_TimerText.isActiveAndEnabled) m_TimerText.gameObject.SetActive(false);
                    f_GetInventory();
                }
                else {
                    // 残り時間をカウントダウン
                    m_RechargeTime = m_NextFreeTicket.Subtract(DateTime.Now);
                    m_TimerText.text = string.Format("{0:00}:{1:00}", m_RechargeTime.Minutes, m_RechargeTime.Seconds);
                }
            }
        }

        if (m_EnergyAmount > 0) {
            if (!m_PlayButton.IsInteractable()) m_PlayButton.interactable = true;
            if (!m_RetryButton.IsInteractable()) m_RetryButton.interactable = true;
        }
        else {
            if (m_PlayButton.IsInteractable()) m_PlayButton.interactable = false;
            if (m_RetryButton.IsInteractable()) m_RetryButton.interactable = false;
        }
        m_EnergyAmountText.text = m_EnergyAmount.ToString();
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================

    public void f_GetInventory() {
        CurrencyManager_Manager.m_Instance.f_GetCurrency();
    }

    public void f_CheckEnergy(VirtualCurrencyRechargeTime p_RechargeDetail) {
        if (p_RechargeDetail == null) {
            return;
        }
        else {
            if (m_EnergyAmount < p_RechargeDetail.RechargeMax) {
                if (!m_TimerText.isActiveAndEnabled) m_TimerText.gameObject.SetActive(true);
                m_NextFreeTicket = DateTime.Now.AddSeconds(p_RechargeDetail.SecondsToRecharge);
                m_RechargeTime = m_NextFreeTicket.Subtract(DateTime.Now);
                m_IsStamCapped = false;
            }
            else {
                if (m_TimerText.isActiveAndEnabled) m_TimerText.gameObject.SetActive(false);
                m_IsStamCapped = true;
            }
        }
    }
}
