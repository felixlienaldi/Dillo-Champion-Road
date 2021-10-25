using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Debug_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Debug_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public AudioSource m_BGM;
    public List<GameObject> m_IngameUI;
    public GameObject m_Dillo;
    public GameObject m_Enemy;
    public GameObject m_Tapfx;
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
        if (Input.GetKeyDown(KeyCode.Q)) {
            f_MuteBGM();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            Player_GameObject.m_Instance.f_Attack(false);
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            Player_GameObject.m_Instance.f_Attack(true);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            f_HideUI();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            f_HideDillo();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            f_HideEnemy();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            f_HideTap();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            f_GiveBerry();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            f_ResetBerry();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            f_FeverWith2xDuration();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            f_FeverWith5xDuration();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            f_Invicible();
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            f_ResetFever();
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            f_DecreaseTo1Hp();
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            f_AddHpToMax();
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_MuteBGM() {
        if (m_BGM.volume == 0) m_BGM.volume = 1;
        else m_BGM.volume = 0;
    }

    public void f_HideUI() {
        for (int i = 0; i < m_IngameUI.Count; i++) {
            if (m_IngameUI[i].activeSelf) m_IngameUI[i].SetActive(false);
            else m_IngameUI[i].SetActive(true);
        }
    }

    public void f_HideDillo() {
        if (m_Dillo.activeSelf) m_Dillo.SetActive(false);
        else m_Dillo.SetActive(true);
    }

    public void f_HideEnemy() {
        if (m_Enemy.activeSelf) m_Enemy.SetActive(false);
        else m_Enemy.SetActive(true);
    }

    public void f_HideTap() {
        if (m_Tapfx.activeSelf) m_Tapfx.SetActive(false);
        else m_Tapfx.SetActive(true);
    }

    public void f_GiveBerry() {
        CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", 1000000);
    }

    public void f_ResetBerry() {
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 1000000);
    }

    public void f_FeverWith2xDuration() {
        Player_GameObject.m_Instance.m_FeverTimeMultiplier = 2;
        Player_GameObject.m_Instance.f_ChangetoFever();
    }

    public void f_FeverWith5xDuration() {
        Player_GameObject.m_Instance.m_FeverTimeMultiplier = 5;
        Player_GameObject.m_Instance.f_ChangetoFever();
    }

    public void f_ResetFever() {
        Player_GameObject.m_Instance.m_FeverTimeMultiplier = 1;
        Player_GameObject.m_Instance.m_CurrentFeverTimer = 0;
    }

    public void f_Invicible() {
        if (Player_GameObject.m_Instance.m_Invincible) Player_GameObject.m_Instance.m_Invincible = false;
        else Player_GameObject.m_Instance.m_Invincible = true;
    }

    public void f_DecreaseTo1Hp() {
        Player_GameObject.m_Instance.f_SetHP(1);
        for (int i = 4; i > 0; i--) {
            UIManager_Manager.m_Instance.f_MinHp(i);
        }
    }

    public void f_AddHpToMax() {
        Player_GameObject.m_Instance.f_SetHP(5);
        for (int i = 1; i <=5; i++) {
            UIManager_Manager.m_Instance.f_AddHP(i);
        }
    }
}
