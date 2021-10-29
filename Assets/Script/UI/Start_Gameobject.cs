using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Start_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Start_Gameobject m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public GameObject m_Parent;
    public AudioClip m_CountdownClip;
    public AudioClip m_StartClip;
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
    public void f_StartCountdown() {
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("EN", 1);
        Player_GameObject.m_Instance.f_Reset();
        GameManager_Manager.m_Instance.f_ApplyPotion();
        GameManager_Manager.m_Instance.f_ApplyBuff();
    }
    public void f_PlayCountDown() {
        Audio_Manager.m_Instance.f_PlayOneShot(m_CountdownClip);
    }

    public void f_CountdownDone() {
        m_Parent.gameObject.SetActive(false);
        Audio_Manager.m_Instance.f_PlayOneShot(m_StartClip);
        GameManager_Manager.m_Instance.f_Initialize();
    }
}
