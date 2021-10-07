using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObjectPooler;

public class Leaderboard_Manager : PoolingManager_Manager<Leaderboard_Gameobject>{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Leaderboard_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public Transform m_Parent;
    public Leaderboard_Gameobject m_PlayerLeaderboard;
    //===== PRIVATES =====
    Leaderboard_Gameobject t_Objects;
    Vector3 t_Vector;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Spawn(string p_Pos,string p_Name,string p_Score) {
        t_Objects = f_SpawnObject();
        t_Objects.transform.parent = m_Parent;
        t_Vector = t_Objects.transform.position;
        t_Vector.z = -100;
        t_Objects.transform.position = t_Vector;
        t_Objects.f_Init(p_Pos, p_Name, p_Score);
        t_Objects.gameObject.SetActive(true);
    }

    public void f_DeactivateAllLeaderboard() {
        for (int i = 0; i < m_PoolingContainer.Count; i++) {
            m_PoolingContainer[i].gameObject.SetActive(false);
        }
    }

    public void f_UpdatePlayer(string p_Pos,string p_Name,string p_Score) {
        m_PlayerLeaderboard.f_Init(p_Pos, p_Name, p_Score);
    }
}
