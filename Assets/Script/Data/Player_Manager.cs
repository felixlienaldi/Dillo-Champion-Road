using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Player_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public int m_Berry;
    public int m_HighScore;
    public string m_Names;
    public string m_Pos;
    public int m_EquipedClothes =0;
    public int m_MatchCount = 0;
    public bool m_BoughAds = false;
    public GameObject m_Notice;
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
        UIManager_Manager.m_Instance.f_BerryTextUpdate(m_Berry);
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_AcceptRating() {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }

    public void f_OpenRating() {
        m_MatchCount++;
        if (m_MatchCount >= 2) {
            if (!PlayerPrefs.HasKey("Reviewed")) {
                m_Notice.SetActive(true);
                PlayerPrefs.SetString("Reviewed","Reviewed");
            }
        }
    }
}
