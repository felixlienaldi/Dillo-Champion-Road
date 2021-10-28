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
    public TextMeshProUGUI m_BerryScore;
    public GameObject m_Highscore;
    public AudioClip m_HighScoreClip;
    public Button m_2XButton;
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

        DailyMission_Manager.m_Instance.f_AddCurrentMatch();
        DailyMission_Manager.m_Instance.f_Endgame();
        PowerupUI_Manager.m_Instance.f_SaveDataPotion();

        m_Highscore.SetActive(GameManager_Manager.m_Instance.m_Score > Player_Manager.m_Instance.m_HighScore);
        if (GameManager_Manager.m_Instance.m_Score > Player_Manager.m_Instance.m_HighScore) {
            PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("Highscore",(int) GameManager_Manager.m_Instance.m_Score);
            Audio_Manager.m_Instance.f_PlayOneShot(m_HighScoreClip);
        }
        m_TotalScore.text = GameManager_Manager.m_Instance.m_Score.ToString("00") + "pt";
        CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", Mathf.FloorToInt((GameManager_Manager.m_Instance.m_Score / 100)));
        m_BerryScore.text = "+"+Mathf.FloorToInt((GameManager_Manager.m_Instance.m_Score/100)).ToString();
        m_EndGameUI.SetActive(true);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
        m_2XButton.interactable = true;
    }

    public void f_Success() {
        m_2XButton.interactable = false;
        m_BerryScore.text = "+" + (Mathf.FloorToInt((GameManager_Manager.m_Instance.m_Score / 100)) * 2).ToString();
        CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE", Mathf.FloorToInt((GameManager_Manager.m_Instance.m_Score / 100)));
    }

    public void f_Retry() {
        
    }

    public void f_MainMenu() {

    }
}
