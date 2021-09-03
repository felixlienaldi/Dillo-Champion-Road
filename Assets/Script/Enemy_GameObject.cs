using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enumerator;

public class Enemy_GameObject : Character_GameObject {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====

    //===== PUBLIC =====
    public ENEMY_TYPE m_Type;
    public int m_SpawnPositionIndex;
    public List<Transform> m_ListGrids;
    public float m_Speed;
    public float m_ScoreValue;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){

    }

    void Start(){
        
    }

    void Update(){
       
    }

    public void OnTriggerEnter2D(Collider2D p_Collision) {
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public override void f_Move() {
        m_SpawnPositionIndex--;
        transform.position = m_ListGrids[m_SpawnPositionIndex].position;
    }
  

}
