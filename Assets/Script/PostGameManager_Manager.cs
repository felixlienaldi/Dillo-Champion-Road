using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enumerator;
public class PostGameManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PostGameManager_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public GameObject m_EndGameUI;
    public TextMeshProUGUI m_TotalScore;
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
    public void f_EndGame() {
        GameManager_Manager.m_Instance.m_GameState = GAME_STATE.ENDGAME;
        GameManager_Manager.m_Instance.m_ListActiveEnemies.Clear();
        m_TotalScore.text = "Total Score : " + GameManager_Manager.m_Instance.m_Score;
        UIManager_Manager.m_Instance.m_TextImage.text = "";
        m_EndGameUI.SetActive(true);
    }

    public void f_Retry() {
        m_EndGameUI.SetActive(false);
        MainMenuManager_Manager.m_Instance.f_ShowHTP();
    }
    public void f_MainMenu() {
        m_EndGameUI.SetActive(false);
        MainMenuManager_Manager.m_Instance.f_Initialize();
    }
}
