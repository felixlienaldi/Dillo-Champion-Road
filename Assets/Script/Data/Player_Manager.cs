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
}
