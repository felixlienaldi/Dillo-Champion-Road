using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyMission_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static DailyMission_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    [Header("Mission 1 (Play X Match)")]
    public int m_RequiredPlayMatch = 5;
    public int m_CurrentPlayMatch = 0;

    [Header("Mission 2 (Destroy X Enemy)")]
    public int m_RequiredDestroyedEnemy = 500;
    public int m_CurrentDestroyedEnemy = 0;

    [Header("Mission 3 (Achieve X Combo)")]
    public int m_RequiredCombo = 250;
    public int m_CurrentCombo = 0;

    [Header("Mission 4 (Destroy X Enemy)")]
    public int m_RequiredEnemy = 100;
    public int m_CurrentEnemy = 0;
    public int m_EnemyID = 0;
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
}
