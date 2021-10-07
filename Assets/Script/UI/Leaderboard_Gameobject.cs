using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====
    //===== PUBLIC =====
    public TextMeshProUGUI m_PositionText;
    public TextMeshProUGUI m_NameText;
    public TextMeshProUGUI m_ScoreText;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Init(string p_Pos, string p_Name, string p_Score) {
        m_PositionText.text = "#" + (int.Parse(p_Pos) + 1).ToString();
        m_NameText.text = p_Name;
        m_ScoreText.text = p_Score;
    }
}
