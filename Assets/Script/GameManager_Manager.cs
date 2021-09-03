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

    [Header("Score")]
    public float m_Score;
    public float m_ScoreMultiplier;

    [Header("GameObject")]
    public GameObject m_Player;
    public GameObject m_GamePlayUI;

    [Header("List Enemies")]
    public List<Transform> m_LeftGrids;
    public List<Transform> m_RightGrids;
    public List<Enemy_GameObject> m_ListActiveEnemies =  new List<Enemy_GameObject>();
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
        m_Score = 0;
        UIManager_Manager.m_Instance.f_SetScoreText("" + m_Score);
        m_GameState = GAME_STATE.GAME;
        m_Player.SetActive(true);
        for (int i = 0; i < 4; i++) {
            f_Spawn(i);
        }
        m_GamePlayUI.SetActive(true);
    }

    public void f_AddScore(float p_Score) {
        m_Score += f_CalculateScore(p_Score);
        UIManager_Manager.m_Instance.f_SetScoreText("" + m_Score);
    }

    public float f_CalculateScore(float p_Score) {
        return p_Score * m_ScoreMultiplier;
    }

    public void f_PostGameManager() {
        Timing.KillCoroutines();
        PostGameManager_Manager.m_Instance.f_EndGame();
        m_Player.SetActive(false);
        for(int i = 0; i < SpawnManager_Manager.m_Instance.m_ListEnemy.Count; i++) {
            SpawnManager_Manager.m_Instance.m_ListEnemy[i].gameObject.SetActive(false);
        }
        m_GamePlayUI.SetActive(false);
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
        p_Enemy.m_SpawnPositionIndex = p_Index;
    }

    public void f_Spawn(int p_Index) {
        m_ListActiveEnemies.Add(SpawnManager_Manager.m_Instance.f_Spawn());
        f_AssignLine(m_ListActiveEnemies[m_ListActiveEnemies.Count - 1], p_Index);
    }

    public void f_NextLine(Enemy_GameObject p_Enemy) {
        m_ListActiveEnemies.Remove(p_Enemy);
        p_Enemy.gameObject.SetActive(false);
        for (int i = 0; i < m_ListActiveEnemies.Count; i++) {
            m_ListActiveEnemies[i].f_Move();
        }
        f_Spawn(m_LeftGrids.Count - 1);
    }
}
