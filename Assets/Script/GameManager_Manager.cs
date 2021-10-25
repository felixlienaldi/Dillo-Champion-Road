using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
using Enumerator;
public class GameManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static GameManager_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public GAME_STATE m_GameState;
    public bool m_ContinueChance = true;
    public int m_InverseMultiplier =1;
    public int m_NormalMultiplier=1;

    [Header("List Upgrade")]
    public List<Buff_GameObject> m_ListUpgrade;
    public List<Buff_GameObject> m_ListPotion;

    [Header("Score")]
    public float m_Score;
    public float m_ScoreMultiplier;

    [Header("GameObject")]
    public Player_GameObject m_Player;
    public GameObject m_GamePlayUI;
    public GameObject m_HpUI;
    public GameObject m_GameOverUI;
    public GameObject m_ContinueMenu;
    public GameObject m_PostGame;

    [Header("List Enemies")]
    public List<Transform> m_LeftGrids;
    public List<Transform> m_RightGrids;
    public List<Enemy_GameObject> m_ListActiveEnemies =  new List<Enemy_GameObject>();

    [Header("Audio")]
    public AudioClip m_GetScoreClip;
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
    public void f_Initialize() {
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("EN", 1);
        m_Score = 0;
        UIManager_Manager.m_Instance.f_SetScoreText("" + m_Score);
        m_GameState = GAME_STATE.GAME;
        m_HpUI.gameObject.SetActive(true);
        m_Player.f_Reset();
        m_ContinueChance = true;
        f_ApplyBuff();
        for (int i = 0; i < 6; i++) {
            f_Spawn(i);
        }
        m_GamePlayUI.SetActive(true);
        f_ApplyPotion();
    }

    public void f_ApplyBuff() {
        for (int i = 0; i < m_ListUpgrade.Count; i++) {
            if (m_ListUpgrade[i].m_UpgradeType == UPGRADE_TYPE.SCORE) m_ScoreMultiplier = m_ListUpgrade[i].f_GetTotalMultiplier();
            else if (m_ListUpgrade[i].m_UpgradeType == UPGRADE_TYPE.HPGAIN) m_Player.m_MinimumCombo = m_Player.m_DefaultMinimumCombo-m_ListUpgrade[i].f_GetTotalMultiplier();
            else if (m_ListUpgrade[i].m_UpgradeType == UPGRADE_TYPE.FEVERTIME) m_Player.m_FeverTimeMultiplier = m_ListUpgrade[i].f_GetTotalMultiplier();
        }
    }

    public void f_ApplyPotion() {
        Player_GameObject.m_Instance.f_ResetBuffRelated();
        for (int i = 0; i < m_ListPotion.Count; i++) {
            if (m_ListPotion[i].m_UpgradeType == UPGRADE_TYPE.ACCURACY) m_Player.m_AbsoluteHitCount = m_ListPotion[i].m_Value;
            else if (m_ListPotion[i].m_UpgradeType == UPGRADE_TYPE.FEVERGAIN) m_Player.m_FeverGainMultiplier = m_ListPotion[i].m_Value;
            else if (m_ListPotion[i].m_UpgradeType == UPGRADE_TYPE.BARRIER) m_Player.m_BarrierCount = m_ListPotion[i].m_Value;
            else if (m_ListPotion[i].m_UpgradeType == UPGRADE_TYPE.REVIVE) m_Player.m_ReviveCount = m_ListPotion[i].m_Value;
        }
    }

    public void f_AddScore(float p_Score) {
        m_Score += f_CalculateScore(p_Score);
        Audio_Manager.m_Instance.f_PlayOneShot(m_GetScoreClip);
        UIManager_Manager.m_Instance.f_SetScoreText("" + m_Score);
    }

    public float f_CalculateScore(float p_Score) {
        if(m_ListActiveEnemies[0].m_Type == ENEMY_TYPE.INVERSE)   return p_Score * m_ScoreMultiplier * m_InverseMultiplier;
        else  return p_Score * m_ScoreMultiplier * m_NormalMultiplier;
    }

    public void f_PostGameManager() {
        //Timing.KillCoroutines();
        m_HpUI.gameObject.SetActive(false);
        m_GameState = GAME_STATE.ENDGAME;
        m_GameOverUI.gameObject.SetActive(true);
        m_GamePlayUI.SetActive(false);
    }

    public void f_GameOver() {
        if (m_ContinueChance) {
            m_ContinueMenu.gameObject.SetActive(true);
            m_ContinueChance = false;
            Timing.RunCoroutine(ie_ContinueTimer(),"Continue");
        }
        else {
            m_ContinueMenu.gameObject.SetActive(false);
            Timing.KillCoroutines("Continue");
            f_EndGame();
        }
    }

    public void f_EndGame() {
        m_ContinueMenu.gameObject.SetActive(false);
        m_GameOverUI.gameObject.SetActive(false);
        for (int i = 0; i < SpawnManager_Manager.m_Instance.m_ListEnemy.Count; i++) {
            SpawnManager_Manager.m_Instance.m_ListEnemy[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < m_ListPotion.Count; i++) {
            m_ListPotion[i].m_Applied = false;
            m_ListPotion[i].m_Bought= false;
        }
        m_ListPotion.Clear();
        PostGameManager_Manager.m_Instance.f_EndGame();
        m_ListActiveEnemies.Clear();
    }

    public void f_Continue() {
        Timing.KillCoroutines("Continue");
        m_HpUI.gameObject.SetActive(true);
        Player_GameObject.m_Instance.f_Reset();
        m_GameState = GAME_STATE.GAME;
        m_GameOverUI.gameObject.SetActive(false);
        m_ContinueMenu.gameObject.SetActive(false);        
        m_GamePlayUI.SetActive(true);
    }

    public void f_AssignLine(Enemy_GameObject p_Enemy, int p_Index) {
        if (p_Enemy.m_SpawnPositionIndex == 1) {
            p_Enemy.transform.position = m_LeftGrids[p_Index].position;
            p_Enemy.m_ListGrids = m_LeftGrids;
        }
        else {
            p_Enemy.transform.position = m_RightGrids[p_Index].position;
            p_Enemy.m_ListGrids = m_RightGrids;
        }
        p_Enemy.f_LineAssigned();
        p_Enemy.m_SpawnPositionIndex = p_Index;
    }

    public void f_Spawn(int p_Index) {
        m_ListActiveEnemies.Add(SpawnManager_Manager.m_Instance.f_Spawn());
        f_AssignLine(m_ListActiveEnemies[m_ListActiveEnemies.Count - 1], p_Index);
    }

    public void f_NextLine(Enemy_GameObject p_Enemy) {
        m_ListActiveEnemies.Remove(p_Enemy);
        //p_Enemy.gameObject.SetActive(false);
        p_Enemy.m_Animator.SetTrigger("Damage");
        for (int i = 0; i < m_ListActiveEnemies.Count; i++) {
            m_ListActiveEnemies[i].f_Move();
        }
        f_Spawn(m_LeftGrids.Count - 1);
    }

    public void f_Pause() {
        Time.timeScale = 0;
        m_GameState = GAME_STATE.PAUSED;
        m_GamePlayUI.gameObject.SetActive(false);
    }

    public void f_Unpause() {
        Time.timeScale = 1;
        m_GameState = GAME_STATE.GAME;
        m_GamePlayUI.gameObject.SetActive(true);
    }

    public void f_BackToHome() {
        Player_GameObject.m_Instance.m_FeverEffect.SetActive(false);
        Player_GameObject.m_Instance.f_Reset();
        Player_GameObject.m_Instance.m_FeverEnviorment.gameObject.SetActive(false);
        Player_GameObject.m_Instance.m_NormalEnviorment.gameObject.SetActive(true);
        CameraGameObject_GameObject.m_Instance.f_Reset();
        Time.timeScale = 1;
        m_GameState = GAME_STATE.MENU;
        m_HpUI.gameObject.SetActive(false);
        UIManager_Manager.m_Instance.f_ChangeFever(false);
        for (int i = 0; i < SpawnManager_Manager.m_Instance.m_ListEnemy.Count; i++) {
            SpawnManager_Manager.m_Instance.m_ListEnemy[i].gameObject.SetActive(false);
        }

        m_ListActiveEnemies.Clear();
    }

    IEnumerator<float> ie_ContinueTimer() {
        float m_ContinueTimer = 5f;
        do {
            yield return Timing.WaitForSeconds(Time.deltaTime);
            m_ContinueTimer -= Time.deltaTime;
            UIManager_Manager.m_Instance.f_SetContinueTimerFill(m_ContinueTimer);
        } while (m_ContinueTimer > 0);
        m_ContinueMenu.gameObject.SetActive(false);
        f_EndGame();
    }
}
