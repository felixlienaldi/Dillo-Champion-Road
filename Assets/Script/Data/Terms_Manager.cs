using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Terms_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Terms_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public GameObject m_TermsMenu;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        if (PlayerPrefs.HasKey("TermsAccepted")) {
            m_TermsMenu.SetActive(false);
        }
    }

    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Accept() {
        PlayerPrefs.SetString("TermsAccepted", "TermsAccepted");
    }

    public void f_OpenTermPolicy() {
        Application.OpenURL("https://redrain-studio.com/privacy_policies/");
    }
}
