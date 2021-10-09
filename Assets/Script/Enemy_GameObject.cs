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
    public SpriteRenderer m_Sp;
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
    public void f_Init() {
        if (transform.position.x < Player_GameObject.m_Instance.transform.position.x) {
            f_Flip(false);
        }
        else {
            f_Flip(true);
        }
    }
    public override void f_Flip(bool p_Right) {
        if (p_Right) {
            m_Sp.flipX = false;
        }
        else {
            m_Sp.flipX = true;
        }
    }

    public void f_getHit() {
        gameObject.SetActive(false);
    }

    public override void f_Move() {
        m_SpawnPositionIndex--;
        transform.position = m_ListGrids[m_SpawnPositionIndex].position;
    }
  

}
