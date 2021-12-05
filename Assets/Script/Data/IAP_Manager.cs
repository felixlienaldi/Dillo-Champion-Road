using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IAP_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static IAP_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public GameObject m_AdsButton;
    public GameObject m_OutofStockButton;
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
    public void f_AddBerry(int p_Amount) {
        CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", p_Amount);
    }

    public void f_RemoveAds() {
        PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("Ads", "1");
    }

    public void f_AdRemoved() {
        m_AdsButton.SetActive(false);
        m_OutofStockButton.SetActive(true);
    }
}
